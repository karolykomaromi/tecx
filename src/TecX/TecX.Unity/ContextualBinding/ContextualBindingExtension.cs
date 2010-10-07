using System;

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace TecX.Unity.ContextualBinding
{
    public class ContextualBindingExtension : UnityContainerExtension, IContextualBindingConfiguration
    {
        #region Fields

        private readonly IRequestHistory _history;

        #endregion Fields

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextualBindingExtension"/> class
        /// </summary>
        public ContextualBindingExtension()
        {
            _history = new RequestHistory();
        }

        #endregion c'tor

        #region Overrides of UnityContainerExtension

        protected override void Initialize()
        {
            var setup = new ContextualBindingSetupStrategy(_history);

            Context.Strategies.Add(setup, UnityBuildStage.Setup);

            var postInit = new ContextualBindingPostInitStrategy(_history);

            Context.Strategies.Add(postInit, UnityBuildStage.PostInitialization);

            Context.Registering += OnRegistering;
        }

        #endregion Overrides of UnityContainerExtension

        private void OnRegistering(object sender, RegisterEventArgs e)
        {
            //we are only interested in the default mappings
            if (e.Name == null)
            {
                NamedTypeBuildKey key = new NamedTypeBuildKey(e.TypeTo, null);

                var existing = Context.Policies.Get<IBuildPlanPolicy>(key);

                var policy = existing as ContextualBindingBuildPlanPolicy;

                if (policy != null)
                {
                    while (policy.Next != null)
                    {
                        policy = policy.Next;
                    }

                    //TODO weberse can't find out wether the new registration was done
                    //for contextual binding or just a regular default binding which would
                    //simply overwrite our previous registrations -> where can I find out if
                    //I need to do something about this registration?
                }
            }
        }

        #region Implementation of IContextualBindingConfiguration

        public IContextualBindingConfiguration Register<TTo, TFrom>(Func<IRequest, bool> shouldResolveTo)
        {
            ContextualBindingBuildPlanPolicy policy = new ContextualBindingBuildPlanPolicy(
                typeof (TTo),
                typeof (TFrom),
                shouldResolveTo,
                _history);

            //get the default build key for the target type
            NamedTypeBuildKey key = NamedTypeBuildKey.Make<TTo>(null);

            var p = Context.Policies.Get<IBuildPlanPolicy>(key);

            if (p != null)
            {
                ContextualBindingBuildPlanPolicy cb = p as ContextualBindingBuildPlanPolicy;

                if (cb != null)
                {
                    policy.Next = cb;
                }
                else
                {
                    policy.LastChance = p;
                }
            }

            Context.Policies.Set<IBuildPlanPolicy>(policy, key);

            return this;
        }

        #endregion Implementation of IContextualBindingConfiguration
    }
}