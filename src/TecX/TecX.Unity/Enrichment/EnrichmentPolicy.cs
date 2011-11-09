namespace TecX.Unity.Enrichment
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    public class EnrichmentPolicy<T> : IEnrichmentPolicy
        where T : class
    {
        private readonly Action<T, IBuilderContext> enrichWith;

        public EnrichmentPolicy(Action<T, IBuilderContext> enrichWith)
        {
            Guard.AssertNotNull(enrichWith, "enrichWith");

            this.enrichWith = enrichWith;
        }

        public void Enrich(IBuilderContext context)
        {
            Guard.AssertNotNull(context, "context");

            T existing = context.Existing as T;

            if (existing != null)
            {
                this.enrichWith(existing, context);
            }
        }
    }
}
