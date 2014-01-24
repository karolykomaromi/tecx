namespace Infrastructure.Modularity
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using AutoMapper;
    using Infrastructure.I18n;
    using Infrastructure.ViewModels;
    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Modularity;
    using Microsoft.Practices.Prism.Regions;
    using Microsoft.Practices.Unity;

    public abstract class UnityModule : IModule
    {
        private readonly IUnityContainer container;
        private readonly ILoggerFacade logger;
        private readonly IModuleInitializer initializer;

        protected UnityModule(IUnityContainer container, ILoggerFacade logger, IModuleInitializer initializer)
        {
            Contract.Requires(container != null);
            Contract.Requires(logger != null);
            Contract.Requires(initializer != null);

            this.container = container;
            this.logger = logger;
            this.initializer = initializer;
        }

        protected ILoggerFacade Logger
        {
            get { return this.logger; }
        }

        public virtual void Initialize()
        {
            this.initializer.Initialize(this);
        }

        protected internal virtual IResourceManager CreateResourceManager()
        {
            Contract.Ensures(Contract.Result<IResourceManager>() != null);

            return new EchoResourceManager();
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

            return new ResourceDictionary();
        }

        protected virtual bool TryGetViewFor<TViewModel>(out FrameworkElement view)
            where TViewModel : ViewModel
        {
            string viewTypeName = typeof(TViewModel).AssemblyQualifiedName.Replace("ViewModel", "View");

            Type viewType = Type.GetType(viewTypeName, false);

            string message;
            if (viewType != null)
            {
                try
                {
                    view = this.container.Resolve(viewType) as FrameworkElement;
                }
                catch (ResolutionFailedException ex)
                {
                    message = string.Format("Could not resolve view of Type '{0}'.\r\n{1}", viewTypeName, ex);
                    this.Logger.Log(message, Category.Exception, Priority.High);

                    view = null;
                    return false;
                }

                if (view != null)
                {
                    ViewModel vm;

                    try
                    {
                        vm = this.container.Resolve<TViewModel>();
                    }
                    catch (ResolutionFailedException ex)
                    {
                        message = string.Format("Could not resolve view model of Type '{0}'.\r\n{1}", typeof(TViewModel).AssemblyQualifiedName, ex);
                        this.Logger.Log(message, Category.Exception, Priority.High);
                        view = null;
                        return false;
                    }

                    message = string.Format("Successfully created view (Type='{0}') and view model (Type='{1}').", typeof(TViewModel).AssemblyQualifiedName, viewTypeName);
                    this.Logger.Log(message, Category.Info, Priority.Low);
                    view.DataContext = vm;
                    return true;
                }
            }

            message = string.Format("Could not resolve view (Type='{0}') for view model (Type='{1}').", viewTypeName, typeof(TViewModel).AssemblyQualifiedName);
            this.Logger.Log(message, Category.Warn, Priority.High);
            view = null;
            return false;
        }
    }
}
