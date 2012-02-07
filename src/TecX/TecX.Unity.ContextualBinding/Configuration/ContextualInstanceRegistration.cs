namespace TecX.Unity.ContextualBinding.Configuration
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;
    using TecX.Unity.Configuration;

    public class ContextualInstanceRegistration : InstanceRegistration
    {
        private readonly Predicate<IBindingContext, IBuilderContext> predicate;

        public ContextualInstanceRegistration(
            Type @from,
            string name,
            object instance,
            LifetimeManager lifetime,
            Predicate<IBindingContext, IBuilderContext> predicate)
            : base(@from, name, instance, lifetime)
        {
            Guard.AssertNotNull(predicate, "predicate");

            this.predicate = predicate;
        }

        public override void Configure(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            container.RegisterInstance(this.From, this.Instance, this.predicate, this.Lifetime);
        }
    }
}