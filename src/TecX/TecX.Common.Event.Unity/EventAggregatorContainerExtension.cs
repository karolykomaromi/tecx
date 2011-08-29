using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using System.Windows.Threading;

namespace TecX.Common.Event.Unity
{
    public class EventAggregatorContainerExtension : UnityContainerExtension
    {
        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAggregatorContainerExtension"/> class
        /// </summary>
        public EventAggregatorContainerExtension()
        {
#if SILVERLIGHT
            Dispatcher dispatcher = Deployment.Current.Dispatcher;
#else
            Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
#endif
            
            IEventAggregator eventAggregator = new EventAggregator(dispatcher);

            _eventAggregator = eventAggregator;
        }

        #region Overrides of UnityContainerExtension

        /// <summary>
        /// Initial the container with this extension's functionality.
        /// </summary>
        /// <remarks>
        /// When overridden in a derived class, this method will modify the given
        ///             <see cref="T:Microsoft.Practices.Unity.ExtensionContext"/> by adding strategies, policies, etc. to
        ///             install it's functions into the container.
        /// </remarks>
        protected override void Initialize()
        {
            Context.Container.RegisterInstance(_eventAggregator, new ExternallyControlledLifetimeManager());

            EventAggregatorSubscriptionStrategy strategy = new EventAggregatorSubscriptionStrategy(_eventAggregator);

            Context.Strategies.Add(strategy, UnityBuildStage.PostInitialization);
        }

        #endregion
    }
}
