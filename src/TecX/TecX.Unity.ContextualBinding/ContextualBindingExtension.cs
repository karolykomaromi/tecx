namespace TecX.Unity.ContextualBinding
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    using TecX.Common;

    public class ContextualBindingExtension : UnityContainerExtension, IContextualBindingConfigurator
    {
        private readonly Dictionary<NamedTypeBuildKey, ContextualBuildKeyMappingPolicy> mappings;

        private readonly Dictionary<string, object> bindingContext;

        private readonly BuildTreeTrackerStrategy treeTracker;

        public ContextualBindingExtension()
        {
            this.mappings = new Dictionary<NamedTypeBuildKey, ContextualBuildKeyMappingPolicy>();
            this.bindingContext = new Dictionary<string, object>();
            this.treeTracker = new BuildTreeTrackerStrategy();
        }

        public object this[string key]
        {
            get
            {
                Guard.AssertNotEmpty(key, "key");

                object value;
                if (this.BindingContext.TryGetValue(key, out value))
                {
                    return value;
                }

                return null;
            }

            set
            {
                Guard.AssertNotEmpty(key, "key");

                this.BindingContext[key] = value;
            }
        }

        public IDictionary<NamedTypeBuildKey, ContextualBuildKeyMappingPolicy> Mappings
        {
            get
            {
                return this.mappings;
            }
        }

        public IDictionary<string, object> BindingContext
        {
            get
            {
                return this.bindingContext;
            }
        }

        public BuildTreeTrackerStrategy TreeTracker
        {
            get
            {
                return this.treeTracker;
            }
        }

        public void RegisterType(
            Type from,
            Type to,
            Predicate<IBindingContext, IBuilderContext> isMatch,
            LifetimeManager lifetime,
            params InjectionMember[] injectionMembers)
        {
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(to, "to");
            Guard.AssertNotNull(isMatch, "isMatch");

            // if no lifetime manager is registered we use the transient lifetime (new instance is created for
            // every resolve). this is identical to the unity default behavior
            if (lifetime == null)
            {
                lifetime = new TransientLifetimeManager();
            }

            ContextualBuildKeyMappingPolicy policy = this.GetPolicy(from);

            string uniqueMappingName = Guid.NewGuid().ToString();

            // add another contextual mapping to the policy
            policy.AddMapping(isMatch, to, uniqueMappingName);

            // by registering our mapping under a name that is guaranteed to be unique we can make use
            // of the full container infrastructure that is already in place
            Container.RegisterType(from, to, uniqueMappingName, lifetime, injectionMembers);
        }

        public void RegisterInstance(
            Type from, 
            object instance, 
            Predicate<IBindingContext, IBuilderContext> matches, 
            LifetimeManager lifetime)
        {
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(instance, "instance");
            Guard.AssertNotNull(matches, "isMatch");

            if (lifetime == null)
            {
                lifetime = new ContainerControlledLifetimeManager();
            }

            ContextualBuildKeyMappingPolicy policy = this.GetPolicy(from);

            string uniqueMappingName = Guid.NewGuid().ToString();

            // add another contextual mapping to the policy. this is a speciality of Unity. seems like they call
            // it "identityKey" where from -> to.
            // see UnityDefaultBehaviorExtension.OnRegisterInstance
            policy.AddMapping(matches, from, uniqueMappingName);

            // by registering our mapping under a name that is guaranteed to be unique we can make use
            // of the full container infrastructure that is already in place
            Container.RegisterInstance(from, uniqueMappingName, instance, lifetime);
        }

        protected override void Initialize()
        {
            this.Context.Registering += this.OnRegistering;

            this.Context.Strategies.Add(this.TreeTracker, UnityBuildStage.PreCreation);
        }

        private void OnRegistering(object sender, RegisterEventArgs e)
        {
            // we are only interested in default mappings
            if (!string.IsNullOrEmpty(e.Name))
            {
                return;
            }

            NamedTypeBuildKey key = new NamedTypeBuildKey(e.TypeFrom);

            ContextualBuildKeyMappingPolicy policy;
            if (this.Mappings.TryGetValue(key, out policy))
            {
                // if something is already registered -> check if we have a registration with same name
                // do the replace and last chance hookup if neccessary
                IBuildKeyMappingPolicy existingPolicy = Context.Policies.Get<IBuildKeyMappingPolicy>(key);
                ContextualBuildKeyMappingPolicy existingContextualPolicy = existingPolicy as ContextualBuildKeyMappingPolicy;

                if (existingPolicy != null && 
                    existingContextualPolicy == null)
                {
                    // means that there is a default mapping registered but its not a contextual mapping
                    policy.DefaultMapping = existingPolicy;
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

                if (existingPolicy != null && 
                    existingContextualPolicy == null)
                {
                    // means that there is a default mapping registered but its not a contextual mapping
                    policy.DefaultMapping = existingPolicy;
                }

                Mappings[key] = policy;

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
            private readonly ContextualBindingExtension extension;

            public DefaultBindingContext(ContextualBindingExtension extension)
            {
                Guard.AssertNotNull(extension, "extension");

                this.extension = extension;
            }

            public BuildTreeNode CurrentBuildNode
            {
                get
                {
                    return this.Extension.TreeTracker.CurrentBuildNode;
                }
            }

            private ContextualBindingExtension Extension
            {
                get
                {
                    return this.extension;
                }
            }

            public object this[string key]
            {
                get
                {
                    Guard.AssertNotEmpty(key, "key");

                    return this.Extension.BindingContext[key];
                }

                set
                {
                    Guard.AssertNotEmpty(key, "key");

                    this.Extension.BindingContext[key] = value;
                }
            }
        }
    }
}