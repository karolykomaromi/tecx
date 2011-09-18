using System;
using Microsoft.Practices.ObjectBuilder2;
using TecX.Common;

namespace TecX.Unity.Enrichment
{
    public class EnrichmentPolicy<T> : IEnrichmentPolicy
        where T : class
    {
        private readonly Action<T, IBuilderContext> _enrichWith;

        public EnrichmentPolicy(Action<T, IBuilderContext> enrichWith)
        {
            Guard.AssertNotNull(enrichWith, "enrichWith");

            _enrichWith = enrichWith;
        }

        public void Enrich(IBuilderContext context)
        {
            Guard.AssertNotNull(context, "context");

            T existing = context.Existing as T;

            if (existing != null)
            {
                _enrichWith(existing, context);
            }
        }
    }
}
