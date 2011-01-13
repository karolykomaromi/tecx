using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

#if SILVERLIGHT

#else
using Microsoft.Practices.Unity;
using Microsoft.Windows.Controls.Ribbon;
#endif

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

#if SILVERLIGHT

#else
            _container.RegisterType<IShowViewModels, ShowViewModelService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IDisplayText, DisplayTextService>(new ContainerControlledLifetimeManager());

            IRegion mainToolBar = _regionManager.Regions[Regions.MainToolBar];

            RibbonButton btnAddStory = new RibbonButton {Label = "Add Story", Command = Commands.AddStoryCard };
            RibbonButton btnRemoveStory = new RibbonButton {Label = "Remove Story", Command = Commands.RemoveStoryCard};

            RibbonGroup grp = new RibbonGroup { Header = "Story Management" };
            grp.Items.Add(btnAddStory);
            grp.Items.Add(btnRemoveStory);

            RibbonTab tab = new RibbonTab
                                {
                                    Header = "Project"
                                };
            tab.Items.Add(grp);

            mainToolBar.Add(tab);
            
#endif

            IRegion main = _regionManager.Regions[Regions.Main];

            main.Add(new Surface());
        }

        #endregion Implementation of IModule
    }
}
