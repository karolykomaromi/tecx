namespace Infrastructure.Client.Test.TestObjects
{
    using System.Windows.Controls;
    using Infrastructure.Modularity;
    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Regions;
    using Microsoft.Practices.Unity;

    public class ViewResolutionFailsModule : UnityModule
    {
        public ViewResolutionFailsModule(IUnityContainer container, ILoggerFacade logger, IModuleTracker moduleTracker, IRegionManager regionManager)
            : base(container, logger, moduleTracker, regionManager)
        {
        }

        public override string ModuleName
        {
            get { return "ViewModelResolutionFails"; }
        }

        protected override void ConfigureRegions(IRegionManager regionManager)
        {
            Control view;
            this.TryGetViewFor<ViewModel2>(out view);
        }
    }
}