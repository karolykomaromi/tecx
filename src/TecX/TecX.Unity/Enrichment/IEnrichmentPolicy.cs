using Microsoft.Practices.ObjectBuilder2;

namespace TecX.Unity.Enrichment
{
    public interface IEnrichmentPolicy : IBuilderPolicy
    {
        void Enrich(IBuilderContext context);
    }
}