namespace TecX.Unity.Configuration
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;
    using TecX.Unity.ContextualBinding;

    public class ContextualTypeRegistration : TypeRegistration
    {
        private readonly Predicate<IRequest> predicate;

        public ContextualTypeRegistration(Type @from, Type to, string name, LifetimeManager lifetime, Predicate<IRequest> predicate, params InjectionMember[] enrichments)
            : base(@from, to, name, lifetime, enrichments)
        {
            Guard.AssertNotNull(predicate, "predicate");

            this.predicate = predicate;
        }

        public override void Configure(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            container.RegisterType(this.From, this.To, this.Lifetime, this.predicate, this.Enrichments);
        }
    }
}