using System.Text;
using System.Threading;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

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
            SynchronizationContext context = SynchronizationContext.Current;

            context = context ?? new SynchronizationContext();

            IEventAggregator eventAggregator = new EventAggregator(context);

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
