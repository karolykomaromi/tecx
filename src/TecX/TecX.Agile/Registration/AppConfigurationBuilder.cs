namespace TecX.Agile.Registration
{
    using System.Windows.Threading;

    using TecX.Agile.Messaging;
    using TecX.Event.Unity;
    using TecX.Unity.Collections;
    using TecX.Unity.Configuration;
    using TecX.Unity.Enrichment;

    public class AppConfigurationBuilder : ConfigurationBuilder
    {
        public AppConfigurationBuilder()
        {
            this.Extension<EnrichmentExtension>();
            this.Extension<CollectionResolutionExtension>();
            this.Extension<EventAggregatorExtension>();

            this.For<Dispatcher>().Use<Dispatcher>().Factory(c => Dispatcher.CurrentDispatcher);

            this.For<IMessageChannel>().Use<NullMessageChannel>().AsSingleton();
        }
    }
}
