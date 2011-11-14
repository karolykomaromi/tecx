namespace TecX.Unity.Configuration
{
    using System;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class TypeRegistration : Registration
    {
        private readonly Type to;
        private readonly InjectionMember[] enrichments;

        public TypeRegistration(Type from, Type to, string name, LifetimeManager lifetime, params InjectionMember[] enrichments)
            : base(from, name, lifetime)
        {
            Guard.AssertNotNull(to, "to");

            this.to = to;
            this.enrichments = enrichments;
        }

        public Type To
        {
            get { return this.to; }
        }

        public InjectionMember[] Enrichments
        {
            get { return this.enrichments; }
        }

        public override void Configure(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            container.RegisterType(this.From, this.To, this.Name, this.Lifetime, this.Enrichments);
        }
    }
}