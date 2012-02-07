namespace TecX.Unity.ContextualBinding.Configuration
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;
    using TecX.Unity.Configuration;

    public class ContextualTypeRegistration : TypeRegistration
    {
        private readonly Predicate<IBindingContext, IBuilderContext> predicate;

        public ContextualTypeRegistration(
            Type @from,
            Type to,
            string name,
            LifetimeManager lifetime,
            Predicate<IBindingContext, IBuilderContext> predicate,
            params InjectionMember[] enrichments)
            : base(@from, to, name, lifetime, enrichments)
        {
            Guard.AssertNotNull(predicate, "predicate");

            this.predicate = predicate;
        }

        public override void Configure(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            container.RegisterType(this.From, this.To, this.predicate, this.Lifetime, this.Enrichments);
        }
    }
}