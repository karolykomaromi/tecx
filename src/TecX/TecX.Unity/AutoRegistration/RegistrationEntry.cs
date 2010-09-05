using System;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.AutoRegistration
{
    internal class RegistrationEntry
    {
        private readonly Predicate<Type> _typeFilter;
        private readonly Action<Type, IUnityContainer> _registrator;
        private readonly IUnityContainer _container;

        public RegistrationEntry(
            Predicate<Type> typeFilter,
            Action<Type, IUnityContainer> registrator, 
            IUnityContainer container)
        {
            Guard.AssertNotNull(typeFilter, "typeFilter");
            Guard.AssertNotNull(registrator, "registrator");
            Guard.AssertNotNull(container, "container");

            _typeFilter = typeFilter;
            _registrator = registrator;
            _container = container;
        }

        public void RegisterIfSatisfiesFilter(Type type)
        {
            if (_typeFilter(type))
            {
                _registrator(type, _container);
            }
        }
    }
}