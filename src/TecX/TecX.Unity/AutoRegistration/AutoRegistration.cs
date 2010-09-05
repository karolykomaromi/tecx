using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.AutoRegistration
{
    /// <summary>
    /// Auto Registration extends popular Unity IoC container 
    /// and provides nice fluent syntax to configure rules for automatic types registration
    /// </summary>
    public class AutoRegistration : IAutoRegistration
    {
        #region Fields

        private readonly List<RegistrationEntry> _registrationEntries;
        private readonly List<Predicate<Assembly>> _excludedAssemblyFilters;
        private readonly List<Predicate<Type>> _excludedTypeFilters;

        private readonly IUnityContainer _container;

        private readonly StringBuilder _log;

        #endregion Fields

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoRegistration"/> class.
        /// </summary>
        /// <param name="container">Unity container.</param>
        public AutoRegistration(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            _container = container;

            _registrationEntries = new List<RegistrationEntry>();
            _excludedAssemblyFilters = new List<Predicate<Assembly>>();
            _excludedTypeFilters = new List<Predicate<Type>>();

            _log = new StringBuilder(10240);
        }

        #endregion c'tor

        /// <summary>
        /// Adds rule to include certain types that satisfy specified type filter
        /// and register them using specified registration options
        /// </summary>
        /// <param name="typeFilter">Type filter.</param>
        /// <param name="registrationBuilder">RegistrationOptions options.</param>
        /// <returns>Auto registration</returns>
        public IAutoRegistration Include(Predicate<Type> typeFilter, RegistrationOptionsBuilder registrationBuilder)
        {
            Guard.AssertNotNull(typeFilter, "typeFilter");
            Guard.AssertNotNull(registrationBuilder, "registrationBuilder");

            _registrationEntries.Add(
                new RegistrationEntry(
                        typeFilter,
                        (type, container) =>
                        {
                            registrationBuilder.ForType(type);

                            foreach (RegistrationOptions registration in registrationBuilder.Build())
                            {
                                _log.AppendFormat(
                                        "Registered mapping from '{0}' to '{1}' with name '{2}' using lifetime '{3}'",
                                        registration.From.Name,
                                        registration.To.Name,
                                        registration.Name,
                                        registration.LifetimeManager.GetType().Name)
                                    .AppendLine();

                                container.RegisterType(
                                    registration.From,
                                    registration.To,
                                    registration.Name,
                                    registration.LifetimeManager);
                            }
                        },
                        _container));
            return this;
        }


        /// <summary>
        /// Loads assembly from given assembly file name.
        /// </summary>
        /// <param name="assemblyFile">Assembly path.</param>
        /// <returns>Auto registration</returns>
        public IAutoRegistration LoadAssemblyFrom(string assemblyFile)
        {
            Guard.AssertNotEmpty(assemblyFile, "assemblyFile");

            if (File.Exists(assemblyFile))
            {
                Assembly assembly = Assembly.LoadFrom(assemblyFile);

                _log.AppendFormat("Loaded assembly {0}", assemblyFile)
                    .AppendLine();
            }
            else
            {
                _log.AppendFormat("Could not find assembly {0}", assemblyFile)
                    .AppendLine();
            }

            return this;
        }

        /// <summary>
        /// Loads assemblies from given assembly file name.
        /// </summary>
        /// <param name="assemblyFiles">Assembly paths.</param>
        /// <returns>Auto registration</returns>
        public IAutoRegistration LoadAssembliesFrom(IEnumerable<string> assemblyFiles)
        {
            Guard.AssertNotNull(assemblyFiles, "assemblyFiles");

            foreach (var assemblyPath in assemblyFiles)
            {
                LoadAssemblyFrom(assemblyPath);
            }

            return this;
        }


        /// <summary>
        /// Adds rule to exclude certain types that satisfy specified type filter and not register them
        /// </summary>
        /// <param name="filter">Type filter.</param>
        /// <returns>Auto registration</returns>
        public IAutoRegistration Exclude(Predicate<Type> filter)
        {
            Guard.AssertNotNull(filter, "filter");

            _excludedTypeFilters.Add(filter);

            return this;
        }

        /// <summary>
        /// Adds rule to exclude certain assemblies that satisfy specified assembly filter
        /// and not consider their types
        /// </summary>
        /// <param name="filter">Type filter.</param>
        /// <returns>Auto registration</returns>
        public IAutoRegistration ExcludeAssemblies(Predicate<Assembly> filter)
        {
            Guard.AssertNotNull(filter, "filter");

            _excludedAssemblyFilters.Add(filter);

            return this;
        }

        /// <summary>
        /// Adds rule to exclude certain assemblies (that name starts with System or mscorlib) 
        /// and not consider their types
        /// </summary>
        /// <returns>Auto registration</returns>
        public IAutoRegistration ExcludeSystemAssemblies()
        {
            ExcludeAssemblies(a => a.GetName().FullName.StartsWith("System") ||
                a.GetName().FullName.StartsWith("mscorlib"));

            return this;
        }

        /// <summary>
        /// Applies auto registration - scans loaded assemblies,
        /// check specified rules and register types that satisfy these rules
        /// </summary>
        public void ApplyAutoRegistration()
        {
            if (_registrationEntries.Any())
            {
                //get all assemblies that are not removed by a filter
                IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(assembly => !_excludedAssemblyFilters
                        .Any(filter => filter(assembly)));

                //get all types in remaining assemblies that are not filtered
                IEnumerable<Type> types = assemblies.SelectMany(assembly => assembly.GetTypes())
                    .Where(type => !_excludedTypeFilters
                        .Any(filter => filter(type)));

                //check if there is any registration for a type
                foreach (Type type in types)
                {
                    foreach (RegistrationEntry entry in _registrationEntries)
                    {
                        entry.RegisterIfSatisfiesFilter(type);
                    }
                }
            }
        }
    }
}