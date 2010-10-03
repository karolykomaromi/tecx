﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Windows.Controls.Ribbon;

using TecX.Agile.Infrastructure;
using TecX.Common;
using TecX.Common.Event;

namespace TecX.Agile.Modules.Main
{
    public class Module : IModule
    {
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

            TextBlock txt = new TextBlock { Text = "Click me!" };
            RibbonButton btn = new RibbonButton { Content = txt };
            RibbonGroup grp = new RibbonGroup { Header = "Header" };
            grp.Items.Add(btn);
            RibbonTab tab = new RibbonTab
                                {
                                    Header = "My first RibbonTab"
                                };
            tab.Items.Add(grp);

            mainToolBar.Add(tab);
        }

        #endregion Implementation of IModule
    }
}
