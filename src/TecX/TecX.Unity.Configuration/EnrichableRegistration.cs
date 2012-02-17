namespace TecX.Unity.Configuration
{
    using System;

    using Microsoft.Practices.Unity;

    public abstract class EnrichableRegistration : Registration
    {
        private readonly InjectionMember[] enrichments;

        protected EnrichableRegistration(Type @from, string name, LifetimeManager lifetime, params InjectionMember[] enrichments)
            : base(@from, name, lifetime)
        {
            this.enrichments = enrichments;
        }

        public InjectionMember[] Enrichments
        {
            get { return this.enrichments; }
        }
    }
}