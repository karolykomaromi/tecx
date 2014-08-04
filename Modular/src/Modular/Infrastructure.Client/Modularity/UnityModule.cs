namespace Infrastructure.Modularity
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Windows.Controls;
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
        private readonly IRegionManager regionManager;

        protected UnityModule(IUnityContainer container, ILoggerFacade logger, IModuleTracker moduleTracker, IRegionManager regionManager)
        {
            Contract.Requires(container != null);
            Contract.Requires(logger != null);
            Contract.Requires(moduleTracker != null);
            Contract.Requires(regionManager != null);

            this.container = container;
            this.logger = logger;
            this.moduleTracker = moduleTracker;
            this.regionManager = regionManager;

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

        protected IRegionManager RegionManager
        {
            get { return this.regionManager; }
        }

        public virtual void Initialize()
        {
            this.ConfigureRegions(this.RegionManager);

            this.moduleTracker.RecordModuleInitialized(this.ModuleName);
        }

        protected virtual void ConfigureRegions(IRegionManager regionManager)
        {
            Contract.Requires(regionManager != null);
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
