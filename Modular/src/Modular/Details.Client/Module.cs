namespace Details
{
    using Infrastructure;
    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Regions;
    using Microsoft.Practices.Unity;

    public class Module : UnityModule
    {
        public Module(IUnityContainer container, IRegionManager regionManager, ILoggerFacade logger)
            : base(container, regionManager, logger)
        {
        }
    }
}
