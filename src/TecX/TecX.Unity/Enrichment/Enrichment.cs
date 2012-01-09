namespace TecX.Unity.Enrichment
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class Enrichment<T> : InjectionMember
        where T : class
    {
        private readonly Action<T, IBuilderContext> enrichWith;

        public Enrichment(Action<T, IBuilderContext> enrichWith)
        {
            Guard.AssertNotNull(enrichWith, "enrichWith");

            this.enrichWith = enrichWith;
        }

        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            Guard.AssertNotNull(implementationType, "implementationType");

            NamedTypeBuildKey buildKey = new NamedTypeBuildKey(implementationType, name);

            var policy = new EnrichmentPolicy<T>(this.enrichWith);

            policies.Set<IEnrichmentPolicy>(policy, buildKey);
        }
    }
}
