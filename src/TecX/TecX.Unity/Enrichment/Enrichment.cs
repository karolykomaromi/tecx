namespace TecX.Unity.Enrichment
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class Enrichment<T> : InjectionMember
        where T : class
    {
        private readonly Action<T, IBuilderContext> enrich;

        public Enrichment(Action<T, IBuilderContext> enrich)
        {
            Guard.AssertNotNull(enrich, "enrichWith");

            this.enrich = enrich;
        }

        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            Guard.AssertNotNull(implementationType, "implementationType");

            NamedTypeBuildKey buildKey = new NamedTypeBuildKey(implementationType, name);

            var policy = new EnrichmentPolicy<T>(this.enrich);

            policies.Set<IEnrichmentPolicy>(policy, buildKey);
        }
    }
}
