namespace Infrastructure.Client.Test.TestObjects
{
    using System.Windows;

    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Regions;
    using Microsoft.Practices.Unity;

    public class NoViewModule : UnityModule
    {
        public NoViewModule(IUnityContainer container, IRegionManager regionManager, ILoggerFacade logger, IApplicationResources applicationResources)
            : base(container, regionManager, logger, applicationResources)
        {
        }

        protected override void ConfigureRegions(IRegionManager regionManager)
        {
            FrameworkElement view;
            this.TryGetViewFor<NoView>(out view);
        }
    }
}