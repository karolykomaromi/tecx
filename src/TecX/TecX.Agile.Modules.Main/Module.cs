using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

#if SILVERLIGHT

#else
using Microsoft.Windows.Controls.Ribbon;
#endif

using TecX.Agile.Infrastructure;
using TecX.Agile.Modules.Main.View;
using TecX.Common;

namespace TecX.Agile.Modules.Main
{
    public class Module : IModule
    {
        public const string ModuleName = "Main";

        private readonly IRegionManager _regionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="Module"/> class
        /// </summary>
        public Module(IRegionManager regionManager)
        {
            Guard.AssertNotNull(regionManager, "regionManager");

            _regionManager = regionManager;
        }

        #region Implementation of IModule

        /// <summary>
        /// Notifies the module that it has be initialized.
        /// </summary>
        public void Initialize()
        {

#if SILVERLIGHT

#else

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
