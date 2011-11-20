namespace TecX.Common.Event.Unity
{
    using System.Windows.Threading;

    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class EventAggregatorExtension : UnityContainerExtension
    {
        private readonly IEventAggregator eventAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAggregatorExtension"/> class
        /// </summary>
        public EventAggregatorExtension()
        {
#if SILVERLIGHT
            Dispatcher dispatcher = Deployment.Current.Dispatcher;
#else
            Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
#endif
            this.eventAggregator = new EventAggregator(dispatcher);
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
            Context.Container.RegisterInstance(this.eventAggregator, new ExternallyControlledLifetimeManager());

            EventAggregatorSubscriptionStrategy strategy = new EventAggregatorSubscriptionStrategy(this.eventAggregator);

            Context.Strategies.Add(strategy, UnityBuildStage.PostInitialization);
        }

        #endregion
    }
}
