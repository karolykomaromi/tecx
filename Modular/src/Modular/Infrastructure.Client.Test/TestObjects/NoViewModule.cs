namespace Infrastructure.Client.Test.TestObjects
{
    using System.Windows;
    using Infrastructure.I18n;
    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Regions;
    using Microsoft.Practices.Unity;

    public class NoViewModule : UnityModule
    {
        public NoViewModule(IUnityContainer container, IRegionManager regionManager, ILoggerFacade logger, IAppResourceAppender appResourceAppender, IResourceManager resourceManager)
            : base(container, regionManager, logger, appResourceAppender, resourceManager)
        {
        }

        protected override void ConfigureRegions(IRegionManager regionManager)
        {
            FrameworkElement view;
            this.TryGetViewFor<NoView>(out view);
        }
    }
}