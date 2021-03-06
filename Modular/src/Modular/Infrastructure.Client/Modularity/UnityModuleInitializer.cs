﻿namespace Infrastructure.Modularity
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using AutoMapper;
    using Infrastructure.UnityExtensions.Registration;
    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Modularity;
    using Microsoft.Practices.Unity;

    public class UnityModuleInitializer : Microsoft.Practices.Prism.Modularity.IModuleInitializer
    {
        private readonly IUnityContainer container;
        private readonly IMappingEngine mapper;
        private readonly ILoggerFacade logger;

        public UnityModuleInitializer(IUnityContainer container, IMappingEngine mapper, ILoggerFacade logger)
        {
            Contract.Requires(container != null);
            Contract.Requires(mapper != null);
            Contract.Requires(logger != null);

            this.container = container;
            this.mapper = mapper;
            this.logger = logger;
        }

        public void Initialize(ModuleInfo moduleInfo)
        {
            Contract.Requires(moduleInfo != null);

            IModule moduleInstance = null;
            try
            {
                moduleInstance = this.CreateModule(moduleInfo);

                if (moduleInstance != null)
                {
                    Assembly moduleAssembly = moduleInstance.GetType().Assembly;

                    this.ConfigureContainer(moduleAssembly);

                    if (Application.Current != null && Application.Current.Resources != null)
                    {
                        ResourceDictionary moduleResources = this.CreateModuleResourceDictionary(moduleInstance);

                        Application.Current.Resources.MergedDictionaries.Add(moduleResources);
                    }

                    this.ConfigureAutoMapper(moduleAssembly);

                    //// regions => should be done by module

                    moduleInstance.Initialize();
                }
            }
            catch (Exception ex)
            {
                this.HandleModuleInitializationError(
                    moduleInfo,
                    moduleInstance != null ? moduleInstance.GetType().Assembly.FullName : null,
                    ex);
            }
        }

        public virtual void HandleModuleInitializationError(ModuleInfo moduleInfo, string assemblyName, Exception exception)
        {
            Contract.Requires(moduleInfo != null);
            Contract.Requires(exception != null);

            Exception moduleException;

            if (exception is ModuleInitializeException)
            {
                moduleException = exception;
            }
            else
            {
                if (!string.IsNullOrEmpty(assemblyName))
                {
                    moduleException = new ModuleInitializeException(moduleInfo.ModuleName, assemblyName, exception.Message, exception);
                }
                else
                {
                    moduleException = new ModuleInitializeException(moduleInfo.ModuleName, exception.Message, exception);
                }
            }

            this.logger.Log(moduleException.ToString(), Category.Exception, Priority.High);

            throw moduleException;
        }

        protected virtual IModule CreateModule(ModuleInfo moduleInfo)
        {
            Contract.Requires(moduleInfo != null);
            Contract.Ensures(Contract.Result<IModule>() != null);

            return this.CreateModule(moduleInfo.ModuleType);
        }

        protected virtual IModule CreateModule(string typeName)
        {
            Contract.Requires(!string.IsNullOrEmpty(typeName));
            Contract.Ensures(Contract.Result<IModule>() != null);

            Type moduleType = Type.GetType(typeName, false);

            if (moduleType == null)
            {
                string msg = string.Format(
                    CultureInfo.CurrentCulture,
                    "Unable to retrieve the module type {0} from the loaded assemblies.  You may need to specify a more fully-qualified type name.",
                    typeName);

                throw new ModuleInitializeException(msg);
            }

            return (IModule)this.container.Resolve(moduleType);
        }

        private void ConfigureContainer(Assembly moduleAssembly)
        {
            IRegistrationConvention convention = new CompositeConvention(
                new CommandsConvention(), 
                new OptionsConvention(), 
                new ViewModelConvention(), 
                new ServiceClientConvention());

            foreach (Type type in moduleAssembly.GetExportedTypes())
            {
                convention.RegisterOnMatch(this.container, type);
            }
        }

        private void ConfigureAutoMapper(Assembly moduleAssembly)
        {
            IDictionary<string, Type> viewModels = moduleAssembly
                .GetExportedTypes()
                .Where(type => type.Name.EndsWith("ViewModel", StringComparison.Ordinal))
                .ToDictionary(type => type.Name.Replace("ViewModel", string.Empty).ToUpperInvariant());

            IDictionary<string, Type> entities = moduleAssembly
                .GetExportedTypes()
                .Where(type => type.FullName.IndexOf("Entities", StringComparison.OrdinalIgnoreCase) >= 0)
                .ToDictionary(type => type.Name.ToUpperInvariant());

            foreach (var entity in entities)
            {
                Type viewModelType;
                if (viewModels.TryGetValue(entity.Key, out viewModelType))
                {
                    this.mapper.ConfigurationProvider.CreateTypeMap(entity.Value, viewModelType);
                }
            }
        }

        private ResourceDictionary CreateModuleResourceDictionary(IModule moduleInstance)
        {
            string uriString = "/" + moduleInstance.GetType().Namespace + ".Client;component/Assets/Resources/Resources.xaml";
            Uri source = new Uri(uriString, UriKind.Relative);

            try
            {
                return new ResourceDictionary { Source = source };
            }
            catch (Exception ex)
            {
                string msg = string.Format(
                    CultureInfo.CurrentCulture,
                    "An exception occured while trying to create the default resources for module '{0}' from source '{1}'.\r\n{2}",
                    moduleInstance.GetType().FullName,
                    source,
                    ex);

                this.logger.Log(msg, Category.Exception, Priority.Medium);
            }

            return new ResourceDictionary();
        }
    }
}
