using System;
using System.Diagnostics.Contracts;
using System.Windows;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace Infrastructure.Client
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
            string viewModelName = typeof(TViewModel).AssemblyQualifiedName;

            string viewName = viewModelName.Replace("ViewModel", "View");

            Type viewType = Type.GetType(viewName, false);

            if (viewType != null)
            {
                TViewModel viewModel = this.Container.Resolve<TViewModel>();
                view = this.Container.Resolve(viewType) as FrameworkElement;
                view.DataContext = viewModel;

                return true;
            }

            view = null;
            return false;
        }
    }
}
