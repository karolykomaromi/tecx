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

        //TODO weberse might have to make that thing thread static or at least
        //thread safe in the future
        private readonly IRequestHistory _history;
        private readonly IDictionary<NamedTypeBuildKey, ContextualBuildKeyMappingPolicy> _mappings;
        private readonly IDictionary<NamedTypeBuildKey, IList<LifetimeMapping>> _lifetimeMappings;

        #endregion Fields

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextualBindingExtension"/> class
        /// </summary>
        public ContextualBindingExtension()
        {
            _history = new RequestHistory();
            _mappings = new Dictionary<NamedTypeBuildKey, ContextualBuildKeyMappingPolicy>();
            _lifetimeMappings = new Dictionary<NamedTypeBuildKey, IList<LifetimeMapping>>();
        }

        #endregion c'tor

        protected override void Initialize()
        {
            var setup = new RequestTrackerStrategy(_history);

            Context.Strategies.Add(setup, UnityBuildStage.Setup);

            var lifetime = new ContextualLifetimeStrategy(_history, Context, _lifetimeMappings);
            
            Context.Strategies.Add(lifetime, UnityBuildStage.Lifetime);

            Context.Registering += OnRegister;
        }

        public override void Remove()
        {
            Context.Registering -= OnRegister;
        }

        private void OnRegister(object sender, RegisterEventArgs e)
        {
            //we are only interested in default mappings
            if (e.Name != null)
                return;

            NamedTypeBuildKey key = new NamedTypeBuildKey(e.TypeFrom);

            //if we dont have an override registered we dont have to do
            //anything
            if (!_mappings.ContainsKey(key))
                return;

            var current = Context.Policies.Get<IBuildKeyMappingPolicy>(key);

            //if the currently registered mapping is an override we can leave that one alone
            if (current != null)
            {
                var contextual = current as ContextualBuildKeyMappingPolicy;

                if (contextual != null)
                    return;
            }

            //but if it is not a contextual mapping this means someone else
            //tried to overwrite our mapping so we need to put that one back in place
            var mapping = _mappings[key];

            mapping.LastChance = current;

            Context.Policies.Set<IBuildKeyMappingPolicy>(mapping, key);
        }

        #region Implementation of IContextualBindingConfiguration

        public IContextualBindingConfiguration Register<TFrom, TTo>(Func<IRequest, bool> shouldResolve)
        {
            return Register<TFrom, TTo>(shouldResolve, null);
        }

        public IContextualBindingConfiguration Register<TFrom, TTo>(Func<IRequest, bool> shouldResolve, 
            LifetimeManager lifetimeManager)
        {
            Guard.AssertNotNull(shouldResolve, "shouldResolve");

            NamedTypeBuildKey key = new NamedTypeBuildKey(typeof(TFrom));

            ContextualBuildKeyMappingPolicy policy;
            if (!_mappings.TryGetValue(key, out policy))
            {
                //mapping doesnt exist -> create one and remember it
                policy = new ContextualBuildKeyMappingPolicy(_history);

                _mappings[key] = policy;
            }

            //add the new mapping
            policy.AddMapping(shouldResolve, typeof(TTo));

            var existing = Context.Policies.Get<IBuildKeyMappingPolicy>(key);

            if (existing != null)
            {
                var contextual = existing as ContextualBuildKeyMappingPolicy;

                //if its not already a contextual mapping make the new mapping
                //the last chance handler of our contextual policy
                if (contextual == null)
                {
                    policy.LastChance = existing;
                }
            }

            //TODO check out how the injection members are handled in the
            //UnityDefaultBehaviorStrategy and copy that behavior!
            Context.Policies.Set<IBuildKeyMappingPolicy>(policy, key);

            key = new NamedTypeBuildKey(typeof(TTo));

            IList<LifetimeMapping> lifetimes;
            if(!_lifetimeMappings.TryGetValue(key, out lifetimes))
            {
                lifetimes = new List<LifetimeMapping>();
                _lifetimeMappings[key] = lifetimes;
            }

            lifetimeManager = lifetimeManager ?? new TransientLifetimeManager();

            lifetimes.Add(new LifetimeMapping(shouldResolve, lifetimeManager));

            return this;
        }

        #endregion Implementation of IContextualBindingConfiguration
    }
}