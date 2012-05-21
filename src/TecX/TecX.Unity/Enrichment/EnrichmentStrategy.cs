namespace TecX.Unity.Enrichment
{
    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    public class EnrichmentStrategy : BuilderStrategy
    {
        public override void PreBuildUp(IBuilderContext context)
        {
            Guard.AssertNotNull(context, "context");
            Guard.AssertNotNull(context.BuildKey, "context.BuildKey");

            IEnrichmentPolicy policy = context.Policies.Get<IEnrichmentPolicy>(context.BuildKey);

            if (policy != null)
            {
                policy.Enrich(context);
            }
        }
    }
}
