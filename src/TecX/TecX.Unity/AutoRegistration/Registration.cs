using System;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.AutoRegistration
{
    public class Registration
    {
        private readonly Filter<Type> _filter;
        private readonly Action<Type, IUnityContainer> _registrator;

        public Registration(Filter<Type> filter, Action<Type, IUnityContainer> registrator)
        {
            Guard.AssertNotNull(filter, "filter");
            Guard.AssertNotNull(registrator, "registrator");

            _filter = filter;
            _registrator = registrator;
        }

        public virtual void RegisterIfSatisfiesFilter(Type type, IUnityContainer container)
        {
            if (_filter.IsMatch(type))
                _registrator(type, container);
        }
    }
}
