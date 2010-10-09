using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using System.Reflection;

using TecX.Common;

namespace TecX.Unity.ContextualBinding
{
    public class ContextualBindingExtension : UnityContainerExtension, IContextualBindingConfiguration
    {
        #region Fields

        private readonly IRequestHistory _history;
        private readonly IDictionary<NamedTypeBuildKey, ContextualBindingBuildKeyMappingPolicy> _mappings;

        #endregion Fields

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextualBindingExtension"/> class
        /// </summary>
        public ContextualBindingExtension()
        {
            _history = new RequestHistory();
            _mappings = new Dictionary<NamedTypeBuildKey, ContextualBindingBuildKeyMappingPolicy>();
        }

        #endregion c'tor

        protected override void Initialize()
        {
            var setup = new ContextualBindingSetupStrategy(_history);

            Context.Strategies.Add(setup, UnityBuildStage.Setup);

            var postInit = new ContextualBindingPostInitStrategy(_history);

            Context.Strategies.Add(postInit, UnityBuildStage.PostInitialization);

            Context.Registering += OnRegister;
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
                var contextual = current as ContextualBindingBuildKeyMappingPolicy;

                if (contextual != null)
                    return;
            }

            //but if it is not a contextual mapping this means someone else
            //tried to overwrite our mapping so we need to put that one back in place
            var mapping = _mappings[key];

            var m = mapping;
            while (m.Next != null)
            {
                m = m.Next;
            }

            m.LastChance = current;

            Context.Policies.Set<IBuildKeyMappingPolicy>(mapping, key);
        }

        #region Implementation of IContextualBindingConfiguration

        public IContextualBindingConfiguration Register<TFrom, TTo>(Func<IRequest, bool> shouldResolve)
        {
            Guard.AssertNotNull(shouldResolve, "shouldResolve");

            NamedTypeBuildKey key = new NamedTypeBuildKey(typeof(TFrom));

            ContextualBindingBuildKeyMappingPolicy policy =
                new ContextualBindingBuildKeyMappingPolicy(_history, shouldResolve, typeof(TTo));

            var existing = Context.Policies.Get<IBuildKeyMappingPolicy>(key);

            if (existing != null)
            {
                var contextual = existing as ContextualBindingBuildKeyMappingPolicy;

                if (contextual != null)
                {
                    policy.Next = contextual;
                }
                else
                {
                    policy.LastChance = existing;
                }
            }

            Context.Policies.Set<IBuildKeyMappingPolicy>(policy, key);

            _mappings[key] = policy;

            return this;
        }

        #endregion Implementation of IContextualBindingConfiguration
    }
}