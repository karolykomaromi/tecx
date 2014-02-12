namespace Infrastructure.Client.Test.TestObjects
{
    using System.Windows.Controls;
    using Infrastructure.Modularity;
    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Regions;
    using Microsoft.Practices.Unity;

    public class ViewModelResolutionFailsModule : UnityModule
    {
        public ViewModelResolutionFailsModule(IUnityContainer container, ILoggerFacade logger, IModuleTracker moduleTracker, IModuleInitializer initializer)
            : base(container, logger, moduleTracker, initializer)
        {
        }

        public override string ModuleName
        {
            get { return "ViewModelResolutionFails"; }
        }

        protected override void ConfigureRegions(IRegionManager regionManager)
        {
            Control view;
            this.TryGetViewFor<ViewModel1>(out view);
        }
    }
}