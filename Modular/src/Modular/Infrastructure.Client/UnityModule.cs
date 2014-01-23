﻿namespace Infrastructure
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows;
    using Infrastructure.I18n;
    using Infrastructure.ViewModels;
    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Modularity;
    using Microsoft.Practices.Prism.Regions;
    using Microsoft.Practices.Unity;
    
    public abstract class UnityModule : IModule
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;
        private readonly ILoggerFacade logger;
        private readonly IAppResourceAppender appResourceAppender;
        private readonly IResourceManager resourceManager;

        protected UnityModule(IUnityContainer container, IRegionManager regionManager, ILoggerFacade logger, IAppResourceAppender appResourceAppender, IResourceManager resourceManager)
        {
            Contract.Requires(container != null);
            Contract.Requires(regionManager != null);
            Contract.Requires(logger != null);
            Contract.Requires(appResourceAppender != null);
            Contract.Requires(resourceManager != null);

            this.container = container;
            this.regionManager = regionManager;
            this.logger = logger;
            this.appResourceAppender = appResourceAppender;
            this.resourceManager = resourceManager;
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

        protected IAppResourceAppender AppResourceAppender
        {
            get { return this.appResourceAppender; }
        }

        protected IResourceManager ResourceManager
        {
            get { return this.resourceManager; }
        }

        public virtual void Initialize()
        {
            this.ConfigureContainer(this.container);

            IResourceManager resourceManager = this.CreateResourceManager();
            this.AppResourceAppender.Add(resourceManager);

            ResourceDictionary moduleResources = this.CreateModuleResources();
            this.AppResourceAppender.Add(moduleResources);

            this.ConfigureRegions(this.RegionManager);
        }

        protected virtual IResourceManager CreateResourceManager()
        {
            Contract.Ensures(Contract.Result<IResourceManager>() != null);

            return new EchoResourceManager();
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
                    this.Logger.Log(message, Category.Exception, Priority.High);

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
                        this.Logger.Log(message, Category.Exception, Priority.High);
                        view = null;
                        return false;
                    }

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
