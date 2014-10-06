namespace TecX.Unity.Enrichment
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class EnrichmentExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Context.Strategies.AddNew<EnrichmentStrategy>(UnityBuildStage.PostInitialization);
        }
    }
}
