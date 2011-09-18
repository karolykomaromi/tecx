using System;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using TecX.Common;

namespace TecX.Unity.Enrichment
{
    public class Enrichment<T> : InjectionMember
        where T : class
    {
        private readonly Action<T, IBuilderContext> _enrichWith;

        public Enrichment(Action<T, IBuilderContext> enrichWith)
        {
            Guard.AssertNotNull(enrichWith, "enrichWith");

            _enrichWith = enrichWith;
        }

        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            NamedTypeBuildKey buildKey = new NamedTypeBuildKey(implementationType, name);

            var policy = new EnrichmentPolicy<T>(_enrichWith);

            policies.Set<IEnrichmentPolicy>(policy, buildKey);
        }
    }
}
