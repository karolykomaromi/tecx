namespace TecX.Unity.Enrichment
{
    using Microsoft.Practices.ObjectBuilder2;

    public class EnrichmentStrategy : BuilderStrategy
    {
        public override void PreBuildUp(IBuilderContext context)
        {
            IEnrichmentPolicy policy = context.Policies.Get<IEnrichmentPolicy>(context.BuildKey);

            if (policy != null)
            {
                policy.Enrich(context);
            }
        }
    }
}
