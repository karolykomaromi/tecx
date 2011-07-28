using System;

using Microsoft.Practices.Unity;

namespace TecX.Unity.AutoRegistration
{
    internal class RegistrationEntry
    {
        private readonly Filter<Type> _filter;
        private readonly Action<Type, IUnityContainer> _registrator;
        private readonly IUnityContainer _container;

        public RegistrationEntry(Filter<Type> filter, Action<Type, IUnityContainer> registrator, 
            IUnityContainer container)
        {
            if (filter == null) throw new ArgumentNullException("typeFilter");
            if (registrator == null) throw new ArgumentNullException("registrator");
            if (container == null) throw new ArgumentNullException("container");

            _filter = filter;
            _registrator = registrator;
            _container = container;
        }

        public void RegisterIfSatisfiesFilter(Type type)
        {
            if (_filter.IsMatch(type))
                _registrator(type, _container);
        }
    }
}
