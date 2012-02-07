namespace TecX.Unity.TypedFactory.Configuration
{
    using System;

    using Microsoft.Practices.Unity;

    using TecX.Common;
    using TecX.Unity.Configuration;

    public class FactoryRegistration : Registration
    {
        private readonly ITypedFactoryComponentSelector selector;

        public FactoryRegistration(Type @from, string name, LifetimeManager lifetime, ITypedFactoryComponentSelector selector)
            : base(@from, name, lifetime)
        {
            Guard.AssertNotNull(selector, "selector");

            this.selector = selector;
        }

        public override void Configure(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            container.RegisterFactory(this.From, this.selector);
        }
    }
}