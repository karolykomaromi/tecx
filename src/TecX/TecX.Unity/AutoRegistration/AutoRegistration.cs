﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
        private readonly List<RegistrationEntry> _registrationEntries = new List<RegistrationEntry>();
        
        private readonly List<Predicate<Assembly>> _excludedAssemblyFilters = new List<Predicate<Assembly>>();
        private readonly List<Predicate<Type>> _excludedTypeFilters = new List<Predicate<Type>>();

        private readonly IUnityContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoRegistration"/> class.
        /// </summary>
        /// <param name="container">Unity container.</param>
        public AutoRegistration (IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");
            _container = container;
        }

        /// <summary>
        /// Adds rule to include certain types that satisfy specified type filter
        /// and register them using specified registrator function
        /// </summary>
        /// <param name="typeFilter">Type filter.</param>
        /// <param name="registrator">Registrator function.</param>
        /// <returns>Auto registration</returns>
        public virtual IAutoRegistration Include(
            Predicate<Type> typeFilter,
            Action<Type, IUnityContainer> registrator)
        {
            Guard.AssertNotNull(typeFilter, "typeFilter");
            Guard.AssertNotNull(registrator, "registrator");

            _registrationEntries.Add(new RegistrationEntry(typeFilter, registrator, _container));
            return this;
        }

        /// <summary>
        /// Adds rule to include certain types that satisfy specified type filter
        /// and register them using specified registration options
        /// </summary>
        /// <param name="typeFilter">Type filter.</param>
        /// <param name="registrationOptions">RegistrationOptions options.</param>
        /// <returns>Auto registration</returns>
        public virtual IAutoRegistration Include(
            Predicate<Type> typeFilter,
            IRegistrationOptions registrationOptions)
        {
            Guard.AssertNotNull(typeFilter, "typeFilter");
            Guard.AssertNotNull(registrationOptions, "registrationOptions");

            _registrationEntries.Add(new RegistrationEntry(
                                         typeFilter,
                                         (t, c) =>
                                             {
                                                 registrationOptions.Type = t;
                                                 foreach (var contract in registrationOptions.Interfaces)
                                                 {
                                                     c.RegisterType(
                                                         contract,
                                                         t,
                                                         registrationOptions.Name,
                                                         registrationOptions.LifetimeManager);
                                                 }
                                             },
                                         _container));
            return this;
        }

        /// <summary>
        /// Adds rule to exclude certain assemblies that satisfy specified assembly filter
        /// and not consider their types
        /// </summary>
        /// <param name="filter">Type filter.</param>
        /// <returns>Auto registration</returns>
        public virtual IAutoRegistration ExcludeAssemblies(Predicate<Assembly> filter)
        {
            Guard.AssertNotNull(filter, "filter");

            _excludedAssemblyFilters.Add(filter);
            return this;
        }

        /// <summary>
        /// Adds rule to exclude certain types that satisfy specified type filter and not register them
        /// </summary>
        /// <param name="filter">Type filter.</param>
        /// <returns>Auto registration</returns>
        public virtual IAutoRegistration Exclude(Predicate<Type> filter)
        {
            Guard.AssertNotNull(filter, "filter");

            _excludedTypeFilters.Add(filter);
            return this;
        }

        /// <summary>
        /// Applies auto registration - scans loaded assemblies,
        /// check specified rules and register types that satisfy these rules
        /// </summary>
        public virtual void ApplyAutoRegistration()
        {
            if (_registrationEntries.Any())
                foreach (var type in AppDomain.CurrentDomain
                    .GetAssemblies()
                    .Where(a => !_excludedAssemblyFilters.Any(f => f(a)))
                    .SelectMany(a => a.GetTypes())
                    .Where(t => !_excludedTypeFilters.Any(f => f(t))))
                    foreach (var entry in _registrationEntries)
                        entry.RegisterIfSatisfiesFilter(type);
        }

        private class RegistrationEntry
        {
            private readonly Predicate<Type> _typeFilter;
            private readonly Action<Type, IUnityContainer> _registrator;
            private readonly IUnityContainer _container;

            public RegistrationEntry(Predicate<Type> typeFilter,
                                     Action<Type, IUnityContainer> registrator, IUnityContainer container)
            {
                _typeFilter = typeFilter;
                _registrator = registrator;
                _container = container;
            }

            public void RegisterIfSatisfiesFilter(Type type)
            {
                if (_typeFilter(type))
                    _registrator(type, _container);
            }
        }
    }
}