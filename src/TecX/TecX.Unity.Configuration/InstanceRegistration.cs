using System;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.Configuration
{
    public class InstanceRegistration : Registration
    {
        private readonly object _instance;

        public object Instance
        {
            get { return _instance; }
        }

        public InstanceRegistration(Type from, string name, object instance, LifetimeManager lifetime)
            : base(from, name, lifetime)
             
        {
            Guard.AssertNotNull(instance, "instance");

            _instance = instance;
        }

        public override void Configure(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            container.RegisterInstance(From, Name, Instance, Lifetime);
        }
    }
}