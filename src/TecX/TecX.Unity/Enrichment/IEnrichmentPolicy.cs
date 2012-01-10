namespace TecX.Unity.Enrichment
{
    using Microsoft.Practices.ObjectBuilder2;

    public interface IEnrichmentPolicy : IBuilderPolicy
    {
        void Enrich(IBuilderContext context);
    }
}