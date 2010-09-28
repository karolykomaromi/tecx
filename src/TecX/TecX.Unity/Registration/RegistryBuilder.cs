using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.Registration
{
    public class RegistryBuilder : IRegistry
    {
        #region Fields

        private readonly IUnityContainer _container;
        private readonly List<Filter<Assembly>> _excludeAssemblyFilters;
        private readonly List<Filter<Type>> _excludeTypeFilters;
        private readonly List<Func<IEnumerable<Assembly>>> _assemblyLoaders;
        private readonly List<Registration> _registrations;

        #endregion Fields

        #region c'tor

        public RegistryBuilder(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            _container = container;

            _excludeAssemblyFilters = new List<Filter<Assembly>>();
            _excludeTypeFilters = new List<Filter<Type>>();
            _assemblyLoaders = new List<Func<IEnumerable<Assembly>>>();
            _registrations = new List<Registration>();
        }

        #endregion c'tor

        #region Methods

        public IRegistry Exclude(Filter<Type> filter)
        {
            Guard.AssertNotNull(filter, "filter");

            _excludeTypeFilters.Add(filter);

            return this;
        }

        public IRegistry Exclude(Filter<Assembly> filter)
        {
            Guard.AssertNotNull(filter, "filter");

            _excludeAssemblyFilters.Add(filter);

            return this;
        }

        public IRegistry LoadAssembly(Func<Assembly> assemblyLoader)
        {
            Guard.AssertNotNull(assemblyLoader, "assemblyLoader");

            _assemblyLoaders.Add(() => new[] {assemblyLoader()});

            return this;
        }

        public IRegistry LoadAssemblies(Func<IEnumerable<Assembly>> assemblyLoader)
        {
            Guard.AssertNotNull(assemblyLoader, "assemblyLoader");

            _assemblyLoaders.Add(assemblyLoader);

            return this;
        }

        private static IEnumerable<Assembly> LoadAdditionalAssemblies(
            IEnumerable<Func<IEnumerable<Assembly>>> assemblyLoaders)
        {
            Guard.AssertNotNull(assemblyLoaders, "assemblyLoaders");

            List<Assembly> assemblies = new List<Assembly>();

            foreach (var loader in assemblyLoaders)
            {
                var loadedAssemblies = loader();

                assemblies.AddRange(loadedAssemblies);
            }

            return assemblies;
        }

        public void AddRegistration(Registration registration)
        {
            Guard.AssertNotNull(registration, "registration");

            _registrations.Add(registration);
        }

        public IRegistry ApplyRegistrations()
        {
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies();

            //filter assemblies you want to ignore
            assemblies = assemblies.Where(a => !_excludeAssemblyFilters.Any(f => f.IsMatch(a)));

            //add assemblies you explicitely decided to include
            assemblies = assemblies.Union(LoadAdditionalAssemblies(_assemblyLoaders));

            IEnumerable<Type> types = assemblies.SelectMany(a => a.GetTypes());

            //filter types you chose to ignore explicitely
            types = types.Where(t => !_excludeTypeFilters.Any(f => f.IsMatch(t)));

            foreach (Type type in types)
            {
                foreach (Registration entry in _registrations)
                {
                    entry.RegisterIfSatisfiesFilter(type, _container);
                }
            }

            return this;
        }

        #endregion Methods
    }
}