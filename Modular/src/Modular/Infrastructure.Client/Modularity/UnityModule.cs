using System.Globalization;

namespace Infrastructure.Modularity
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using AutoMapper;
    using Infrastructure.Options;
    using Infrastructure.UnityExtensions.Injection;
    using Infrastructure.ViewModels;
    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Modularity;
    using Microsoft.Practices.Prism.Regions;
    using Microsoft.Practices.Unity;

    public abstract class UnityModule : IModule
    {
        private readonly IUnityContainer container;
        private readonly ILoggerFacade logger;
        private readonly IModuleTracker moduleTracker;
        private readonly IModuleInitializer initializer;

        protected UnityModule(IUnityContainer container, ILoggerFacade logger, IModuleTracker moduleTracker, IModuleInitializer initializer)
        {
            Contract.Requires(container != null);
            Contract.Requires(logger != null);
            Contract.Requires(moduleTracker != null);
            Contract.Requires(initializer != null);

            this.container = container;
            this.logger = logger;
            this.moduleTracker = moduleTracker;
            this.initializer = initializer;

            this.moduleTracker.RecordModuleConstructed(this.ModuleName);
        }

        public abstract string ModuleName { get; }

        protected ILoggerFacade Logger
        {
            get { return this.logger; }
        }

        protected IUnityContainer Container
        {
            get { return this.container; }
        }

        public virtual void Initialize()
        {
            this.initializer.Initialize(this);

            this.moduleTracker.RecordModuleInitialized(this.ModuleName);
        }

        protected internal virtual IOptions CreateModuleOptions()
        {
            Contract.Ensures(Contract.Result<IOptions>() != null);

            Type optionsType = this.GetType().Assembly.GetExportedTypes().FirstOrDefault(typeof(IOptions).IsAssignableFrom);

            if (optionsType != null)
            {
                try
                {
                    return (IOptions)this.Container.Resolve(optionsType);
                }
                catch (ResolutionFailedException ex)
                {
                    string msg = string.Format(
                        CultureInfo.CurrentCulture,
                        "An error occured while trying to resolve the default options for module '{0}'. Options type is '{1}'.\r\n{2}", 
                        this.ModuleName, 
                        optionsType.AssemblyQualifiedName, 
                        ex);

                    this.Logger.Log(msg, Category.Exception, Priority.Medium);
                }
            }

            return new NullOptions();
        }

        protected internal virtual void ConfigureContainer(IUnityContainer container)
        {
            Contract.Requires(container != null);
        }

        protected internal virtual void ConfigureRegions(IRegionManager regionManager)
        {
            Contract.Requires(regionManager != null);
        }

        protected internal virtual void ConfigureMapping(IMappingEngine mappingEngine)
        {
            Contract.Requires(mappingEngine != null);

            Assembly assembly = this.GetType().Assembly;

            IDictionary<string, Type> viewModels =
                assembly.GetExportedTypes()
                        .Where(type => type.Name.EndsWith("ViewModel", StringComparison.Ordinal))
                        .ToDictionary(type => type.Name.Replace("ViewModel", string.Empty).ToUpperInvariant());

            IDictionary<string, Type> entities =
                assembly.GetExportedTypes()
                        .Where(type => type.FullName.IndexOf("Entities", StringComparison.OrdinalIgnoreCase) > -1)
                        .ToDictionary(type => type.Name.ToUpperInvariant());

            foreach (var entity in entities)
            {
                Type viewModelType;
                if (viewModels.TryGetValue(entity.Key, out viewModelType))
                {
                    mappingEngine.ConfigurationProvider.CreateTypeMap(entity.Value, viewModelType);
                }
            }
        }

        protected internal virtual ResourceDictionary CreateModuleResources()
        {
            Contract.Ensures(Contract.Result<ResourceDictionary>() != null);

            string uriString = "/" + this.GetType().Namespace + ".Client;component/Assets/Resources/Resources.xaml";

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
                    this.ModuleName, 
                    source, 
                    ex);

                this.Logger.Log(msg, Category.Exception, Priority.Medium);
            }

            return new ResourceDictionary();
        }

        protected virtual bool TryGetViewFor<TViewModel>(out Control view, params Parameter[] parameters)
            where TViewModel : ViewModel
        {
            string viewTypeName = typeof(TViewModel).AssemblyQualifiedName.Replace("ViewModel", "View");

            Type viewType = Type.GetType(viewTypeName, false);

            string message;
            if (viewType != null)
            {
                try
                {
                    view = this.Container.Resolve(viewType) as Control;
                }
                catch (ResolutionFailedException ex)
                {
                    message = string.Format(CultureInfo.CurrentCulture, "Could not resolve view of Type '{0}'.\r\n{1}", viewTypeName, ex);
                    this.Logger.Log(message, Category.Exception, Priority.High);

                    view = null;
                    return false;
                }

                if (view != null)
                {
                    ViewModel vm;

                    try
                    {
                        if (parameters != null && parameters.Length > 0)
                        {
                            ResolverOverride[] overrides = new ResolverOverride[parameters.Length];

                            for (int i = 0; i < parameters.Length; i++)
                            {
                                Parameter parameter = parameters[i];

                                overrides[i] = new ParameterOverride(parameter.Name, parameter.Value);
                            }

                            vm = this.Container.Resolve<TViewModel>(overrides);
                        }
                        else
                        {
                            vm = this.Container.Resolve<TViewModel>();
                        }
                    }
                    catch (ResolutionFailedException ex)
                    {
                        message = string.Format(CultureInfo.CurrentCulture, "Could not resolve view model of Type '{0}'.\r\n{1}", typeof(TViewModel).AssemblyQualifiedName, ex);
                        this.Logger.Log(message, Category.Exception, Priority.High);
                        view = null;
                        return false;
                    }

                    message = string.Format(CultureInfo.CurrentCulture, "Successfully created view (Type='{0}') and view model (Type='{1}').", typeof(TViewModel).AssemblyQualifiedName, viewTypeName);
                    this.Logger.Log(message, Category.Info, Priority.Low);
                    view.DataContext = vm;
                    ViewModelBinder.Bind(view, vm);
                    return true;
                }
            }

            message = string.Format(CultureInfo.CurrentCulture, "Could not resolve view (Type='{0}') for view model (Type='{1}').", viewTypeName, typeof(TViewModel).AssemblyQualifiedName);
            this.Logger.Log(message, Category.Warn, Priority.High);
            view = null;
            return false;
        }
    }
}
