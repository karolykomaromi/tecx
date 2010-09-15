using System;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.AutoRegistration
{
    internal class Registration
    {
        private readonly Filter<Type> _filter;
        private readonly Action<Type, IUnityContainer> _registrator;
        private readonly IUnityContainer _container;

        public Registration(Filter<Type> filter, Action<Type, IUnityContainer> registrator, 
            IUnityContainer container)
        {
            Guard.AssertNotNull(filter, "filter");
            Guard.AssertNotNull(registrator, "registrator");
            Guard.AssertNotNull(container, "container");

            _filter = filter;
            _registrator = registrator;
            _container = container;
        }

        public virtual void RegisterIfSatisfiesFilter(Type type)
        {
            if (_filter.IsMatch(type))
                _registrator(type, _container);
        }
    }
}
