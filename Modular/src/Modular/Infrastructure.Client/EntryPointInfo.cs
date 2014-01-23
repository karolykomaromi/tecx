namespace Infrastructure
{
    using System.Diagnostics.Contracts;
    using Infrastructure.I18n;
    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Regions;
    using Microsoft.Practices.Unity;

    public class EntryPointInfo : IEntryPointInfo
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;
        private readonly ILoggerFacade logger;
        private readonly IAppResourceAppender resourceAppender;
        private readonly IResourceManager resourceManager;

        public EntryPointInfo(IUnityContainer container, IRegionManager regionManager, ILoggerFacade logger, IAppResourceAppender resourceAppender, IResourceManager resourceManager)
        {
            Contract.Requires(container != null);
            Contract.Requires(regionManager != null);
            Contract.Requires(logger != null);
            Contract.Requires(resourceAppender != null);
            Contract.Requires(resourceManager != null);

            this.container = container;
            this.regionManager = regionManager;
            this.logger = logger;
            this.resourceAppender = resourceAppender;
            this.resourceManager = resourceManager;
        }

        public IUnityContainer Container
        {
            get { return this.container; }
        }

        public IRegionManager RegionManager
        {
            get { return this.regionManager; }
        }

        public ILoggerFacade Logger
        {
            get { return this.logger; }
        }

        public IAppResourceAppender ResourceAppender
        {
            get { return this.resourceAppender; }
        }

        public IResourceManager ResourceManager
        {
            get { return this.resourceManager; }
        }
    }
}