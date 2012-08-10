namespace TecX.Unity.Configuration
{
    using System;

    using Microsoft.Practices.Unity;

    using TecX.Common;
    using TecX.Unity.ContextualBinding;
    using TecX.Unity.Tracking;

    public class ContextualInstanceRegistration : InstanceRegistration
    {
        private readonly Predicate<IRequest> predicate;

        public ContextualInstanceRegistration(Type @from, string name, object instance, LifetimeManager lifetime, Predicate<IRequest> predicate)
            : base(@from, name, instance, lifetime)
        {
            Guard.AssertNotNull(predicate, "predicate");

            this.predicate = predicate;
        }

        public override void Configure(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            container.RegisterInstance(this.From, this.Instance, this.Lifetime, this.predicate);
        }
    }
}