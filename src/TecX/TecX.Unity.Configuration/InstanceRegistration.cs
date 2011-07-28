using System;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.Configuration
{
    public class InstanceRegistration : Registration
    {
        #region Fields

        private readonly object _instance;

        #endregion Fields

        #region Properties

        public object Instance
        {
            get { return _instance; }
        }

        #endregion Properties

        #region c'tor

        public InstanceRegistration(Type from, string name, object instance, LifetimeManager lifetime)
            : base(from, name, lifetime)
        {
            Guard.AssertNotNull(instance, "instance");

            _instance = instance;
        }

        #endregion c'tor

        public override void Configure(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            container.RegisterInstance(From, Name, Instance, Lifetime);
        }
    }
}