using Microsoft.Practices.ObjectBuilder2;

namespace TecX.Unity.Enrichment
{
    public class EnrichmentStrategy : BuilderStrategy
    {
        public override void PreBuildUp(IBuilderContext context)
        {
            base.PreBuildUp(context);

            IEnrichmentPolicy policy = context.Policies.Get<IEnrichmentPolicy>(context.BuildKey);

            if (policy != null)
            {
                policy.Enrich(context);
            }
        }
    }
}
