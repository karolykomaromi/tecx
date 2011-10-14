using System;
using System.Collections.Generic;

using Caliburn.Micro;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Agile
{
    public class UnityBootstrapper<T> : Bootstrapper<T>
    {
        private readonly IUnityContainer _container = new UnityContainer();

        protected IUnityContainer Container
        {
            get
            {
                return _container;
            }
        }

        protected sealed override IEnumerable<object> GetAllInstances(Type service)
        {
            Guard.AssertNotNull(service, "service");

            var instances = _container.ResolveAll(service);

            return instances;
        }

        protected sealed override object GetInstance(Type service, string key)
        {
            Guard.AssertNotNull(service, "service");

            var instance = _container.Resolve(service, key);

            return instance;
        }

        protected sealed override void BuildUp(object instance)
        {
            Guard.AssertNotNull(instance, "instance");

            var builtUp = _container.BuildUp(instance.GetType(), instance);
        }
    }
}