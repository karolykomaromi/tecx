namespace Infrastructure.Client.Test.TestObjects
{
    using System.Windows.Controls;
    using Infrastructure.Modularity;
    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Regions;
    using Microsoft.Practices.Unity;

    public class NoViewModule : UnityModule
    {
        public NoViewModule(IUnityContainer container, ILoggerFacade logger, IModuleTracker moduleTracker, IModuleInitializer initializer)
            : base(container, logger, moduleTracker, initializer)
        {
        }

        public override string ModuleName
        {
            get { return "NoView"; }
        }

        protected override void ConfigureRegions(IRegionManager regionManager)
        {
            Control view;
            this.TryGetViewFor<NoView>(out view);
        }
    }
}