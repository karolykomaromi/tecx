using System;
using System.Diagnostics.Contracts;
using System.Windows;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace Infrastructure
{

    public abstract class UnityModule : IModule
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;
        private readonly ILoggerFacade logger;

        protected UnityModule(IUnityContainer container, IRegionManager regionManager, ILoggerFacade logger)
        {
            Contract.Requires(container != null);
            Contract.Requires(regionManager != null);
            Contract.Requires(logger != null);

            this.container = container;
            this.regionManager = regionManager;
            this.logger = logger;
        }

        protected IUnityContainer Container
        {
            get { return this.container; }
        }

        protected IRegionManager RegionManager
        {
            get { return this.regionManager; }
        }

        protected ILoggerFacade Logger
        {
            get { return this.logger; }
        }

        public virtual void Initialize()
        {
            this.ConfigureContainer(this.container);

            ResourceDictionary moduleResources = this.CreateModuleResources();

            Application.Current.Resources.MergedDictionaries.Add(moduleResources);

            this.ConfigureRegions(this.RegionManager);
        }

        protected virtual void ConfigureContainer(IUnityContainer container)
        {
            Contract.Requires(container != null);
        }

        protected virtual void ConfigureRegions(IRegionManager regionManager)
        {
            Contract.Requires(regionManager != null);
        }

        protected virtual ResourceDictionary CreateModuleResources()
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
                    view = this.Container.Resolve(viewType) as FrameworkElement;
                }
                catch (ResolutionFailedException ex)
                {
                    message = string.Format("Could not resolve view of Type '{0}'.\r\n{1}", viewTypeName, ex);
                    Logger.Log(message, Category.Exception, Priority.High);

                    view = null;
                    return false;
                }

                if (view != null)
                {
                    ViewModel vm;

                    try
                    {
                        vm = this.Container.Resolve<TViewModel>();
                    }
                    catch (ResolutionFailedException ex)
                    {
                        message = string.Format("Could not resolve view model of Type '{0}'.\r\n{1}", typeof(TViewModel).AssemblyQualifiedName, ex);
                        Logger.Log(message, Category.Exception, Priority.High);
                        view = null;
                        return false;
                    }

                    view.DataContext = vm;
                    return true;
                }
            }

            message = string.Format("Could not resolve view (Type='{0}') for view model (Type='{1}').", viewTypeName, typeof(TViewModel).AssemblyQualifiedName);
            Logger.Log(message, Category.Warn, Priority.High);
            view = null;
            return false;
        }
    }
}
