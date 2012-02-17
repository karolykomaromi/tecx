namespace TecX.Unity.Configuration
{
    using System;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class TypeRegistration : EnrichableRegistration
    {
        private readonly Type to;

        public TypeRegistration(Type from, Type to, string name, LifetimeManager lifetime, params InjectionMember[] enrichments)
            : base(from, name, lifetime, enrichments)
        {
            Guard.AssertNotNull(to, "to");

            this.to = to;
        }

        public Type To
        {
            get { return this.to; }
        }

        public override void Configure(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            container.RegisterType(this.From, this.To, this.Name, this.Lifetime, this.Enrichments);
        }
    }
}