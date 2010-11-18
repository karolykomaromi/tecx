using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Windows.Controls.Ribbon;

using TecX.Agile.Infrastructure;
using TecX.Common;
using TecX.Common.Event;

namespace TecX.Agile.Modules.Main
{
    public class Module : IModule
    {
        public const string ModuleName = "Main";

        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Module"/> class
        /// </summary>
        public Module(IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            Guard.AssertNotNull(eventAggregator, "eventAggregator");
            Guard.AssertNotNull(regionManager, "regionManager");

            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
        }

        #region Implementation of IModule

        /// <summary>
        /// Notifies the module that it has be initialized.
        /// </summary>
        public void Initialize()
        {
            IRegion mainToolBar = _regionManager.Regions[Regions.MainToolBar];

            RibbonButton btn = new RibbonButton { Label = "click me!" };
            RibbonGroup grp = new RibbonGroup { Header = "FirstRibbonGroup" };
            grp.Items.Add(btn);
            RibbonTab tab = new RibbonTab
                                {
                                    Header = "Main RibbonTab"
                                };
            tab.Items.Add(grp);

            mainToolBar.Add(tab);
        }

        #endregion Implementation of IModule
    }
}
