namespace Infrastructure
{
    using Infrastructure.I18n;
    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Regions;
    using Microsoft.Practices.Unity;

    public interface IEntryPointInfo
    {
        IUnityContainer Container { get; }

        IRegionManager RegionManager { get; }

        ILoggerFacade Logger { get; }

        IAppResourceAppender ResourceAppender { get; }

        IResourceManager ResourceManager { get; }
    }
}
