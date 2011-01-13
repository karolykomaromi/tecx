using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

using TecX.Agile.Infrastructure;
using TecX.Agile.Infrastructure.Services;
using TecX.Agile.Modules.Main.Services;
using TecX.Agile.Modules.Main.View;
using TecX.Common;

namespace TecX.Agile.Modules.Main
{
    public class Module : IModule
    {
        public const string ModuleName = "Main";

        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="Module"/> class
        /// </summary>
        public Module(IRegionManager regionManager, IUnityContainer container)
        {
            Guard.AssertNotNull(regionManager, "regionManager");
            Guard.AssertNotNull(container, "container");

            _regionManager = regionManager;
            _container = container;
        }

        #region Implementation of IModule

        /// <summary>
        /// Notifies the module that it has be initialized.
        /// </summary>
        public void Initialize()
        {
            _container.RegisterType<IShowViewModels, ShowViewModelService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IDisplayText, DisplayTextService>(new ContainerControlledLifetimeManager());

            IRegion main = _regionManager.Regions[Regions.Main];
            
            main.Add(new Surface());

            IRegion surface = _regionManager.Regions[Regions.Surface];

            InfoTextArea infoTextArea = _container.Resolve<InfoTextArea>();

            surface.Add(infoTextArea);
        }

        #endregion Implementation of IModule
    }
}
