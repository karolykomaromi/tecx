namespace TecX.Unity.Configuration
{
    using System;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class InstanceRegistration : Registration
    {
        private readonly object instance;

        public InstanceRegistration(Type from, string name, object instance, LifetimeManager lifetime)
            : base(from, name, lifetime)
        {
            Guard.AssertNotNull(instance, "instance");

            this.instance = instance;
        }

        public object Instance
        {
            get { return this.instance; }
        }

        public override void Configure(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            container.RegisterInstance(this.From, this.Name, this.Instance, this.Lifetime);
        }
    }
}