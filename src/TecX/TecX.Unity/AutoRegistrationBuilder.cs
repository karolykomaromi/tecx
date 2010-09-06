using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.Unity;

using System.Reflection;

namespace TecX.Unity
{
    public class AutoRegistrationBuilder : IAutoRegistration, IFluentInterface
    {
        #region Fields

        private readonly IUnityContainer _container;

        private readonly List<Filter<Assembly>> _excludeAssemblyFilters;
        private readonly List<Filter<Type>> _excludeTypeFilters;
        private readonly List<Func<IEnumerable<Assembly>>> _assemblyLoaders;

        private readonly List<RegistrationEntry> _registrationEntries;

        #endregion Fields

        #region c'tor

        public AutoRegistrationBuilder(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            _container = container;

            _excludeAssemblyFilters = new List<Filter<Assembly>>();
            _excludeTypeFilters = new List<Filter<Type>>();
            _assemblyLoaders = new List<Func<IEnumerable<Assembly>>>();
            _registrationEntries = new List<RegistrationEntry>();
        }

        #endregion c'tor

        #region Methods

        public IAutoRegistration ExcludeSystemAssemblies()
        {
            _excludeAssemblyFilters.Add(Filters.ForAssemblies.IsSystemAssembly());

            return this;
        }

        public IAutoRegistration Exclude(Filter<Type> filter)
        {
            if (filter == null) throw new ArgumentNullException("filter");

            _excludeTypeFilters.Add(filter);

            return this;
        }

        public IAutoRegistration Exclude(Filter<Assembly> filter)
        {
            if (filter == null) throw new ArgumentNullException("filter");

            _excludeAssemblyFilters.Add(filter);

            return this;
        }

        public IAutoRegistration Include(Filter<Type> filter, RegistrationOptionsBuilder builder)
        {
            if (filter == null) throw new ArgumentNullException("filter");
            if (builder == null) throw new ArgumentNullException("builder");

            _registrationEntries.Add(new RegistrationEntry(
                                         filter,
                                         (type, container) =>
                                         {
                                             builder.MappingTo(type);

                                             foreach (RegistrationOptions registration in builder.Build())
                                             {
                                                 container.RegisterType(
                                                     registration.From,
                                                     registration.To,
                                                     registration.Name,
                                                     registration.LifetimeManager,
                                                     registration.InjectionMembers);
                                             }
                                         },
                                         _container));
            return this;
        }

        public IAutoRegistration LoadAssembly(Func<Assembly> assemblyLoader)
        {
            if (assemblyLoader == null) throw new ArgumentNullException("assemblyLoader");

            _assemblyLoaders.Add(() => new[] { assemblyLoader() });

            return this;
        }

        public IAutoRegistration LoadAssemblies(Func<IEnumerable<Assembly>> assemblyLoader)
        {
            if (assemblyLoader == null) throw new ArgumentNullException("assemblyLoader");

            _assemblyLoaders.Add(assemblyLoader);

            return this;
        }

        public IEnumerable<Assembly> LoadAdditionalAssemblies(IEnumerable<Func<IEnumerable<Assembly>>> assemblyLoaders)
        {
            if (assemblyLoaders == null) throw new ArgumentNullException("assemblyLoaders");

            List<Assembly> assemblies = new List<Assembly>();

            foreach (var loader in assemblyLoaders)
            {
                var loadedAssemblies = loader();

                assemblies.AddRange(loadedAssemblies);
            }

            return assemblies;
        }

        public void ApplyAutoRegistrations()
        {
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies();

            //filter assemblies you want to ignore
            assemblies = assemblies.Where(a => !_excludeAssemblyFilters.Any(f => f.IsMatch(a)));

            //add assemblies you explicitely decided to include
            assemblies = assemblies.Union(LoadAdditionalAssemblies(_assemblyLoaders));

            IEnumerable<Type> types = assemblies.SelectMany(a => a.GetTypes());

            //filter types you want to ignore
            types = types.Where(t => !_excludeTypeFilters.Any(f => f.IsMatch(t)));

            foreach (Type type in types)
            {
                foreach (RegistrationEntry entry in _registrationEntries)
                {
                    entry.RegisterIfSatisfiesFilter(type);
                }
            }
        } 

        #endregion Methods
    }
}
