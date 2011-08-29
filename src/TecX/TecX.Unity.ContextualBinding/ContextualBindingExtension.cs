using System;
using System.Collections.Generic;

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using TecX.Common;

namespace TecX.Unity.ContextualBinding
{
    public class ContextualBindingExtension : UnityContainerExtension, IContextualBindingConfiguration
    {
        #region Fields

        private readonly Dictionary<NamedTypeBuildKey, ContextualBuildKeyMappingPolicy> _mappings;
        private readonly Dictionary<string, object> _bindingContext;
        private readonly BuildTreeTrackerStrategy _treeTracker;

        #endregion Fields

        #region Properties

        public object this[string key]
        {
            get
            {
                Guard.AssertNotEmpty(key, "key");

                object value;
                if (BindingContext.TryGetValue(key, out value))
                {
                    return value;
                }

                return null;
            }

            set
            {
                Guard.AssertNotEmpty(key, "key");

                BindingContext[key] = value;
            }
        }

        public IDictionary<NamedTypeBuildKey, ContextualBuildKeyMappingPolicy> Mappings
        {
            get
            {
                return _mappings;
            }
        }

        public IDictionary<string, object> BindingContext
        {
            get
            {
                return _bindingContext;
            }
        }

        public BuildTreeTrackerStrategy TreeTracker
        {
            get
            {
                return _treeTracker;
            }
        }


        #endregion Properties

        #region c'tor

        public ContextualBindingExtension()
        {
            _mappings = new Dictionary<NamedTypeBuildKey, ContextualBuildKeyMappingPolicy>();
            _bindingContext = new Dictionary<string, object>();
            _treeTracker = new BuildTreeTrackerStrategy();
        }

        #endregion c'tor

        #region Implementation of IContextualBindingConfiguration

        public void RegisterType(
            Type from,
            Type to,
            Predicate<IBindingContext, IBuilderContext> matches,
            LifetimeManager lifetime,
            params InjectionMember[] injectionMembers)
        {
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(to, "to");
            Guard.AssertNotNull(matches, "matches");

            // if no lifetime manager is registered we use the transient lifetime (new instance is created for
            // every resolve). this is identical to the unity default behavior
            if (lifetime == null)
            {
                lifetime = new TransientLifetimeManager();
            }

            ContextualBuildKeyMappingPolicy policy = GetPolicy(from);

            string uniqueMappingName = Guid.NewGuid().ToString();

            // add another contextual mapping to the policy
            policy.AddMapping(matches, to, uniqueMappingName);

            // by registering our mapping under a name that is guaranteed to be unique we can make use
            // of the full container infrastructure that is already in place
            Container.RegisterType(from, to, uniqueMappingName, lifetime, injectionMembers);
        }

        public void RegisterInstance(
            Type from, object instance, Predicate<IBindingContext, IBuilderContext> matches, LifetimeManager lifetime)
        {
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(matches, "matches");
            Guard.AssertNotNull(instance, "instance");

            if (lifetime == null)
            {
                lifetime = new ContainerControlledLifetimeManager();
            }

            ContextualBuildKeyMappingPolicy policy = GetPolicy(from);

            string uniqueMappingName = Guid.NewGuid().ToString();

            // add another contextual mapping to the policy. this is a speciality of Unity. seems like they call
            // it "identityKey" where from -> to.
            // see UnityDefaultBehaviorExtension.OnRegisterInstance
            policy.AddMapping(matches, from, uniqueMappingName);

            // by registering our mapping under a name that is guaranteed to be unique we can make use
            // of the full container infrastructure that is already in place
            Container.RegisterInstance(from, uniqueMappingName, instance, lifetime);
        }

        #endregion Implementation of IContextualBindingConfiguration

        #region Overrides of UnityContainerExtension

        protected override void Initialize()
        {
            Context.Registering += OnRegistering;

            Context.Strategies.Add(TreeTracker, UnityBuildStage.PreCreation);
        }

        #endregion Overrides of UnityContainerExtension

        #region Helper

        private void OnRegistering(object sender, RegisterEventArgs e)
        {
            //we are only interested in default mappings
            if (!string.IsNullOrEmpty(e.Name)) return;

            NamedTypeBuildKey key = new NamedTypeBuildKey(e.TypeFrom);

            ContextualBuildKeyMappingPolicy policy;
            if (this.Mappings.TryGetValue(key, out policy))
            {
                //if something is already registered -> check if we have a registration with same name
                //do the replace and last chance hookup if neccessary
                IBuildKeyMappingPolicy existingPolicy = Context.Policies.Get<IBuildKeyMappingPolicy>(key);
                ContextualBuildKeyMappingPolicy existingContextualPolicy =
                    existingPolicy as ContextualBuildKeyMappingPolicy;

                if (existingPolicy != null && existingContextualPolicy == null)
                {
                    //means that there is a default mapping registered but its not a contextual mapping
                    policy.LastChance = existingPolicy;
                }

                Context.Policies.Set<IBuildKeyMappingPolicy>(policy, key);
            }
        }

        private ContextualBuildKeyMappingPolicy GetPolicy(Type from)
        {
            NamedTypeBuildKey key = new NamedTypeBuildKey(from);

            IBuildKeyMappingPolicy existingPolicy = Context.Policies.Get<IBuildKeyMappingPolicy>(key);
            ContextualBuildKeyMappingPolicy existingContextualPolicy = existingPolicy as ContextualBuildKeyMappingPolicy;

            ContextualBuildKeyMappingPolicy policy;
            if (!this.Mappings.TryGetValue(key, out policy))
            {
                // no existing contextual mapping policy for this build key so we have to create an
                // new one and hook it up
                policy = new ContextualBuildKeyMappingPolicy(new DefaultBindingContext(this));

                if (existingPolicy != null && existingContextualPolicy == null)
                {
                    // means that there is a default mapping registered but its not a contextual mapping
                    policy.LastChance = existingPolicy;
                }

                this.Mappings[key] = policy;

                Context.Policies.Set<IBuildKeyMappingPolicy>(policy, key);
            }

            return policy;
        }

        /// <summary>
        /// The default context is more or less a direct link to the extensions data store. The <see cref="UnityContainer"/> does the same
        /// thing with the default implementation of <see cref="ExtensionContext"/> which is a private, nested class called 
        /// <i>ExtensionContextImpl</i>. Looked neat so I applied the same approach here.
        /// </summary>
        private class DefaultBindingContext : IBindingContext
        {
            private readonly ContextualBindingExtension _extension;

            public DefaultBindingContext(ContextualBindingExtension extension)
            {
                Guard.AssertNotNull(extension, "extension");

                _extension = extension;
            }

            public object this[string key]
            {
                get
                {
                    Guard.AssertNotEmpty(key, "key");

                    return Extension.BindingContext[key];
                }

                set
                {
                    Guard.AssertNotEmpty(key, "key");

                    Extension.BindingContext[key] = value;
                }
            }

            public BuildTreeNode CurrentBuildNode
            {
                get
                {
                    return Extension.TreeTracker.CurrentBuildNode;
                }
            }

            private ContextualBindingExtension Extension
            {
                get
                {
                    return _extension;
                }
            }
        }

        #endregion Helper
    }
}