using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using System.Windows.Threading;

namespace TecX.Common.Event.Unity
{
    public class EventAggregatorContainerExtension : UnityContainerExtension,
        IEventAggregatorConfiguration
    {
        private readonly IEventAggregator _eventAggregator;

        public IEventAggregator EventAggregator
        {
            get { return _eventAggregator; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAggregatorContainerExtension"/> class
        /// </summary>
        public EventAggregatorContainerExtension()
        {
            //TODO weberse evaluate using the application dispatcher instead via ctor injection to the
            //container extension
            Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
            
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

            Context.Strategies.AddNew<EventAggregatorSubscriptionStrategy>(UnityBuildStage.PostInitialization);
        }

        #endregion
    }
}
