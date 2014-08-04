namespace TecX.Unity.Enrichment
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    public class EnrichmentPolicy<T> : IEnrichmentPolicy
        where T : class
    {
        private readonly Action<T, IBuilderContext> enrich;

        public EnrichmentPolicy(Action<T, IBuilderContext> enrich)
        {
            Guard.AssertNotNull(enrich, "enrich");

            this.enrich = enrich;
        }

        public void Enrich(IBuilderContext context)
        {
            Guard.AssertNotNull(context, "context");

            T existing = context.Existing as T;

            if (existing != null)
            {
                this.enrich(existing, context);
            }
        }
    }
}
