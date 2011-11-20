namespace TecX.Agile.Registration
{
    using Microsoft.Practices.Unity;

    using TecX.Event.Unity;
    using TecX.Unity.Collections;
    using TecX.Unity.Configuration;
    using TecX.Unity.Enrichment;
    using TecX.Unity.TypedFactory;

    public class AppConfigurationBuilder : ConfigurationBuilder
    {
        public AppConfigurationBuilder()
        {
            AddExpression(x => x.AddModification(container => container.AddNewExtension<TypedFactoryExtension>()));
            AddExpression(x => x.AddModification(container => container.AddNewExtension<EnrichmentExtension>()));
            AddExpression(x => x.AddModification(container => container.AddNewExtension<CollectionResolutionExtension>()));
            AddExpression(x => x.AddModification(container => container.AddNewExtension<EventAggregatorExtension>()));
        }
    }
}
