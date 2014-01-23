namespace Infrastructure.Client.Test.TestObjects
{
    using System.Windows;
    using Infrastructure.I18n;
    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Regions;
    using Microsoft.Practices.Unity;

    public class ViewResolutionFailsModule : UnityModule
    {
        public ViewResolutionFailsModule(IUnityContainer container, IRegionManager regionManager, ILoggerFacade logger, IAppResourceAppender appResourceAppender, IResourceManager resourceManager)
            : base(container, regionManager, logger, appResourceAppender, resourceManager)
        {
        }

        protected override void ConfigureRegions(IRegionManager regionManager)
        {
            FrameworkElement view;
            this.TryGetViewFor<ViewModel2>(out view);
        }
    }
}