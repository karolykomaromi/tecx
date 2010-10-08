using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using System.Reflection;

namespace TecX.Unity.ContextualBinding
{
    public class ContextualBindingExtension : UnityContainerExtension, IContextualBindingConfiguration
    {
        #region Fields

        private readonly IRequestHistory _history;
        private Action<object, RegisterEventArgs> _invoker;

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

            InitPostRegistrationHandler();
        }

        private void InitPostRegistrationHandler()
        {
            FieldInfo field = typeof(UnityContainer).GetField("extensions",
                                                              BindingFlags.NonPublic | BindingFlags.Instance);

            if (field == null) return;

            var extensions = field.GetValue(Container) as List<UnityContainerExtension>;

            if (extensions == null) return;

            UnityDefaultBehaviorExtension ext =
                extensions.OfType<UnityDefaultBehaviorExtension>().SingleOrDefault();

            if (ext == null) return;

            MethodInfo handler = typeof(UnityDefaultBehaviorExtension).GetMethod("OnRegister",
                                                                                 BindingFlags.NonPublic |
                                                                                 BindingFlags.Instance);

            if (handler == null) return;

            EventInfo @event = typeof(UnityContainer).GetEvent("registering",
                                                               BindingFlags.NonPublic |
                                                               BindingFlags.Instance);

            if (@event == null) return;

            Delegate onRegisteringHandler = Delegate.CreateDelegate(@event.EventHandlerType, ext,
                                                                    handler);

            @event.GetRemoveMethod(true).Invoke(Container, new object[] {onRegisteringHandler});

            _invoker = (s, e) => onRegisteringHandler.DynamicInvoke(s, e);

            //TODO weberse unsubscribed the UnityDefaultBehaviorExtension.OnRegister handler
            //now I need to create a handler on my own that raises an event after it triggered exactly that handler
            //argh!
        }

        #endregion Overrides of UnityContainerExtension

        private void OnRegistering(object sender, RegisterEventArgs e)
        {
            ContextualBindingBuildPlanPolicy cbb = null;

            NamedTypeBuildKey key = new NamedTypeBuildKey(e.TypeFrom, null);

            //we are only interested in the default mappings
            if (e.Name == null)
            {
                var existing = Context.Policies.Get<IBuildPlanPolicy>(key);

                var policy = existing as ContextualBindingBuildPlanPolicy;

                if (policy != null)
                {
                    cbb = policy;
                }
            }

            _invoker(sender, e);

            if(cbb != null)
            {
                while (cbb.Next != null)
                {
                    cbb = cbb.Next;
                }

                var existing = Context.Policies.Get<IBuildPlanPolicy>(key);

                var policy = existing as ContextualBindingBuildPlanPolicy;

                if (policy == null)
                {
                    cbb.LastChance = existing;
                }
            }

        }

        #region Implementation of IContextualBindingConfiguration

        public IContextualBindingConfiguration Register<TTo, TFrom>(Func<IRequest, bool> shouldResolveTo)
        {
            ContextualBindingBuildPlanPolicy policy = new ContextualBindingBuildPlanPolicy(
                typeof(TTo),
                typeof(TFrom),
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