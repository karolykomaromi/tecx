namespace TecX.Agile.Registration
{
    using TecX.Event.Unity;
    using TecX.Unity.Collections;
    using TecX.Unity.Configuration;
    using TecX.Unity.Enrichment;
    using TecX.Unity.TypedFactory;

    public class AppConfigurationBuilder : ConfigurationBuilder
    {
        public AppConfigurationBuilder()
        {
            this.ExtendWithNew<TypedFactoryExtension>();
            this.ExtendWithNew<EnrichmentExtension>();
            this.ExtendWithNew<CollectionResolutionExtension>();
            this.ExtendWithNew<EventAggregatorExtension>();
        }
    }
}
