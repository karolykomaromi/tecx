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
        private readonly Dictionary<string, object> _context;

        #endregion Fields

        #region c'tor

        public ContextualBindingExtension()
        {
            _mappings = new Dictionary<NamedTypeBuildKey, ContextualBuildKeyMappingPolicy>();
            _context = new Dictionary<string, object>();
        }

        #endregion c'tor

        protected override void Initialize()
        {
            Context.Registering += OnRegistering;
        }

        private void OnRegistering(object sender, RegisterEventArgs e)
        {
            //we are only interested in default mappings
            if (!string.IsNullOrEmpty(e.Name))
                return;

            NamedTypeBuildKey key = new NamedTypeBuildKey(e.TypeFrom);

            ContextualBuildKeyMappingPolicy policy;
            if (_mappings.TryGetValue(key, out policy))
            {
                //if something is already registered -> check if we have a registration with same name
                //do the replace and last chance hookup if neccessary
                IBuildKeyMappingPolicy existingPolicy = Context.Policies.Get<IBuildKeyMappingPolicy>(key);
                ContextualBuildKeyMappingPolicy existingContextualPolicy = existingPolicy as ContextualBuildKeyMappingPolicy;

                if (existingPolicy != null &&
                    existingContextualPolicy == null)
                {
                    //means that there is a default mapping registered but its not a contextual mapping
                    policy.LastChance = existingPolicy;
                }

                Context.Policies.Set<IBuildKeyMappingPolicy>(policy, key);
            }
        }

        public void RegisterType(Type from, Type to, Predicate<IBindingContext, IBuilderContext> matches,
                             LifetimeManager lifetime, params InjectionMember[] injectionMembers)
        {
            //guards
            if (from == null) throw new ArgumentNullException("from");
            if (to == null) throw new ArgumentNullException("to");
            if (matches == null) throw new ArgumentNullException("matches");

            //if no lifetime manager is registered we use the transient lifetime (new instance is created for
            //every resolve). this is identical to the unity default behavior
            if (lifetime == null)
            {
                lifetime = new TransientLifetimeManager();
            }

            ContextualBuildKeyMappingPolicy policy = GetPolicy(from);

            string uniqueMappingName = Guid.NewGuid().ToString();

            //add another contextual mapping to the policy
            policy.AddMapping(matches, to, uniqueMappingName);

            //by registering our mapping under a name that is guaranteed to be unique we can make use
            //of the full container infrastructure that is already in place
            Container.RegisterType(from, to, uniqueMappingName, lifetime, injectionMembers);
        }

        public void RegisterInstance(Type from, object instance, Predicate<IBindingContext, IBuilderContext> matches, LifetimeManager lifetime)
        {
            if (from == null) throw new ArgumentNullException("from");
            if (matches == null) throw new ArgumentNullException("matches");
            if (instance == null) throw new ArgumentNullException("instance");

            if (lifetime == null)
            {
                lifetime = new ContainerControlledLifetimeManager();
            }

            ContextualBuildKeyMappingPolicy policy = GetPolicy(from);

            string uniqueMappingName = Guid.NewGuid().ToString();

            //add another contextual mapping to the policy. this is a speciality of Unity. seems like they call
            //it "identityKey" where from -> to.
            //see UnityDefaultBehaviorExtension.OnRegisterInstance
            policy.AddMapping(matches, from, uniqueMappingName);

            //by registering our mapping under a name that is guaranteed to be unique we can make use
            //of the full container infrastructure that is already in place
            Container.RegisterInstance(from, uniqueMappingName, instance, lifetime);
        }

        public void Put(string key, object value)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (string.IsNullOrEmpty(key)) throw new ArgumentException(@"Key must not be empty", "key");

            _context[key] = value;
        }

        private ContextualBuildKeyMappingPolicy GetPolicy(Type from)
        {
            NamedTypeBuildKey key = new NamedTypeBuildKey(from);

            IBuildKeyMappingPolicy existingPolicy = Context.Policies.Get<IBuildKeyMappingPolicy>(key);
            ContextualBuildKeyMappingPolicy existingContextualPolicy = existingPolicy as ContextualBuildKeyMappingPolicy;

            ContextualBuildKeyMappingPolicy policy;
            if (!_mappings.TryGetValue(key, out policy))
            {
                //no existing contextual mapping policy for this build key so we have to create an
                //new one and hook it up
                policy = new ContextualBuildKeyMappingPolicy(new DefaultBindingContext(this));

                if (existingPolicy != null &&
                    existingContextualPolicy == null)
                {
                    //means that there is a default mapping registered but its not a contextual mapping
                    policy.LastChance = existingPolicy;
                }

                _mappings[key] = policy;

                Context.Policies.Set<IBuildKeyMappingPolicy>(policy, key);
            }
            return policy;
        }

        /// <summary>
        /// The default context is more or less a direct link to the extensions data story. The <see cref="UnityContainer"/> does the same
        /// thing with the default implementation of <see cref="ExtensionContext"/> which is a private, nested class called 
        /// <i>ExtensionContextImpl</i>. Looked neat so I applied the same approach here.
        /// </summary>
        private class DefaultBindingContext : IBindingContext
        {
            private readonly ContextualBindingExtension _extension;

            public DefaultBindingContext(ContextualBindingExtension extension)
            {
                if (extension == null) throw new ArgumentNullException("extension");

                _extension = extension;
            }

            public object this[string key]
            {
                get
                {
                    if (key == null) throw new ArgumentNullException("key");
                    if (string.IsNullOrEmpty(key)) throw new ArgumentException(@"Key must not be empty", "key");

                    object value;
                    if (_extension._context.TryGetValue(key, out value))
                    {
                        return value;
                    }

                    return null;
                }
            }

            public void Put(string key, object value)
            {
                _extension.Put(key, value);
            }
        }
    }
}