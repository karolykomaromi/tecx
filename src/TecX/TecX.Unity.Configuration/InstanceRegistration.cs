using System;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.Configuration
{
    public class InstanceRegistration : Registration
    {
        private readonly Type _from;
        private readonly string _name;
        private readonly object _instance;
        private readonly LifetimeManager _lifetime;

        public LifetimeManager Lifetime
        {
            get { return _lifetime; }
        }

        public object Instance
        {
            get { return _instance; }
        }

        public string Name
        {
            get { return _name; }
        }

        public Type From
        {
            get { return _from; }
        }

        public InstanceRegistration(Type from, string name, object instance, LifetimeManager lifetime)
            : base(name)
             
        {
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(instance, "instance");
            Guard.AssertNotNull(lifetime, "lifetime");

            _from = from;
            _instance = instance;
            _lifetime = lifetime;
        }

        public override void Configure(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            container.RegisterInstance(From, Name, Instance, Lifetime);
        }
    }
}