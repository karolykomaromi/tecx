namespace TecX.Unity.ContextualBinding
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    using TecX.Common;
    using TecX.Unity.Tracking;

    public class ContextualBindingExtension : UnityContainerExtension, IContextualBinding
    {
        private readonly Dictionary<NamedTypeBuildKey, ContextualBuildKeyMappingPolicy> mappings;

        private readonly Dictionary<string, object> bindingContext;

        private ITracker tracker;

        public ContextualBindingExtension()
        {
            this.mappings = new Dictionary<NamedTypeBuildKey, ContextualBuildKeyMappingPolicy>();
            this.bindingContext = new Dictionary<string, object>();
        }

        public object this[string key]
        {
            get
            {
                Guard.AssertNotEmpty(key, "key");

                object value;
                if (this.bindingContext.TryGetValue(key, out value))
                {
                    return value;
                }

                return null;
            }

            set
            {
                Guard.AssertNotEmpty(key, "key");

                this.bindingContext[key] = value;
            }
        }

        public void RegisterType(Type @from, Type to, LifetimeManager lifetime, Predicate<IRequest> predicate, params InjectionMember[] injectionMembers)
        {
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(to, "to");
            Guard.AssertNotNull(predicate, "predicate");

            // if no lifetime manager is registered we use the transient lifetime (new instance is created for
            // every resolve). this is identical to the unity default behavior
            if (lifetime == null)
            {
                lifetime = new TransientLifetimeManager();
            }

            ContextualBuildKeyMappingPolicy policy = this.GetPolicy(from);

            string uniqueMappingName = Guid.NewGuid().ToString();

            // add another contextual mapping to the policy
            policy.AddMapping(to, uniqueMappingName, predicate);

            // by registering our mapping under a name that is guaranteed to be unique we can make use
            // of the full container infrastructure that is already in place
            this.Container.RegisterType(from, to, uniqueMappingName, lifetime, injectionMembers);
        }

        public void RegisterInstance(Type @from, object instance, LifetimeManager lifetime, Predicate<IRequest> predicate)
        {
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(instance, "instance");
            Guard.AssertNotNull(predicate, "isMatch");

            if (lifetime == null)
            {
                lifetime = new ContainerControlledLifetimeManager();
            }

            ContextualBuildKeyMappingPolicy policy = this.GetPolicy(from);

            string uniqueMappingName = Guid.NewGuid().ToString();

            // add another contextual mapping to the policy. this is a speciality of Unity. seems like they call
            // it "identityKey" where from -> to.
            // see UnityDefaultBehaviorExtension.OnRegisterInstance
            policy.AddMapping(@from, uniqueMappingName, predicate);

            // by registering our mapping under a name that is guaranteed to be unique we can make use
            // of the full container infrastructure that is already in place
            this.Container.RegisterInstance(from, uniqueMappingName, instance, lifetime);
        }

        public bool Remove(string key)
        {
            Guard.AssertNotEmpty(key, "key");

            return this.bindingContext.Remove(key);
        }

        protected override void Initialize()
        {
            var extension = new TrackingExtension();

            this.tracker = extension;

            this.Container.AddExtension(extension);

            this.Context.Registering += this.OnRegistering;

            this.Context.RegisteringInstance += this.OnRegisteringInstance;

            this.Context.Strategies.Add(new ContextualParameterBindingStrategy(new DefaultRequest(this)), UnityBuildStage.PreCreation);
        }

        private void OnRegistering(object sender, RegisterEventArgs e)
        {
            // we are only interested in default mappings
            if (!string.IsNullOrEmpty(e.Name))
            {
                return;
            }

            NamedTypeBuildKey key = new NamedTypeBuildKey(e.TypeFrom);

            this.AdjustMappingPolicy(key);
        }

        private void OnRegisteringInstance(object sender, RegisterInstanceEventArgs e)
        {
            // we are only interested in default mappings
            if (!string.IsNullOrEmpty(e.Name))
            {
                return;
            }

            NamedTypeBuildKey key = new NamedTypeBuildKey(e.RegisteredType);

            this.AdjustMappingPolicy(key);
        }

        private void AdjustMappingPolicy(NamedTypeBuildKey key)
        {
            ContextualBuildKeyMappingPolicy policy;
            if (this.mappings.TryGetValue(key, out policy))
            {
                // if something is already registered -> check if we have a registration with same name
                // do the replace and last chance hookup if neccessary
                IBuildKeyMappingPolicy existingPolicy = this.Context.Policies.Get<IBuildKeyMappingPolicy>(key);
                ContextualBuildKeyMappingPolicy existingContextualPolicy = existingPolicy as ContextualBuildKeyMappingPolicy;

                if (existingPolicy != null && existingContextualPolicy == null)
                {
                    // means that there is a default mapping registered but its not a contextual mapping
                    policy.DefaultMapping = existingPolicy;
                }

                this.Context.Policies.Set<IBuildKeyMappingPolicy>(policy, key);
            }
        }

        private ContextualBuildKeyMappingPolicy GetPolicy(Type from)
        {
            NamedTypeBuildKey key = new NamedTypeBuildKey(from);

            IBuildKeyMappingPolicy existingPolicy = this.Context.Policies.Get<IBuildKeyMappingPolicy>(key);
            ContextualBuildKeyMappingPolicy existingContextualPolicy = existingPolicy as ContextualBuildKeyMappingPolicy;

            ContextualBuildKeyMappingPolicy policy;
            if (!this.mappings.TryGetValue(key, out policy))
            {
                // no existing contextual mapping policy for this build key so we have to create an
                // new one and hook it up
                policy = new ContextualBuildKeyMappingPolicy(new DefaultRequest(this));

                if (existingPolicy != null &&
                    existingContextualPolicy == null)
                {
                    // means that there is a default mapping registered but its not a contextual mapping
                    policy.DefaultMapping = existingPolicy;
                }

                this.mappings[key] = policy;

                this.Context.Policies.Set<IBuildKeyMappingPolicy>(policy, key);
            }

            return policy;
        }

        /// <summary>
        /// The default context is more or less a direct link to the extensions data store. The <see cref="UnityContainer"/> does the same
        /// thing with the default implementation of <see cref="ExtensionContext"/> which is a private, nested class called 
        /// <i>ExtensionContextImpl</i>. Looked neat so I applied the same approach here.
        /// </summary>
        private class DefaultRequest : IRequest
        {
            private readonly ContextualBindingExtension extension;

            public DefaultRequest(ContextualBindingExtension extension)
            {
                Guard.AssertNotNull(extension, "extension");

                this.extension = extension;
            }

            public IBuilderContext Build { get; set; }

            public BuildTreeNode CurrentBuildNode
            {
                get
                {
                    return this.Extension.tracker.CurrentBuildNode;
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

                    object value;

                    if (this.Extension.bindingContext.TryGetValue(key, out value))
                    {
                        return value;
                    }

                    return null;
                }

                set
                {
                    Guard.AssertNotEmpty(key, "key");

                    this.Extension.bindingContext[key] = value;
                }
            }
        }
    }
}