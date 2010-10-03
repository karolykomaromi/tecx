﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Presentation.Regions;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Composite.UnityExtensions;
using Microsoft.Windows.Controls.Ribbon;

using TecX.Common.Event.Unity;
using TecX.Prism.Regions;
using TecX.Unity.Registration;
using TecX.Agile.Data;

namespace TecX.Agile.Planner
{
    public class Bootstrapper : UnityBootstrapper
    {
        #region Overrides of UnityBootstrapper

        protected override DependencyObject CreateShell()
        {
            Shell shell = Container.Resolve<Shell>();

            //in Silverlight you need to call
            //Application.Current.RootVisual = shell;
            shell.Show();

            return shell;
        }

        protected override void ConfigureContainer()
        {
            //TODO weberse configure logging, maybe wcf automagic, a repository and all the other funny
            //Stuff

            Container
                .ConfigureRegistrations()
                .ExcludeSystemAssemblies()
                .Include(The.Extension<EventAggregatorContainerExtension>())
                .Include(If.Implements<IRepository>(),
                    Then.Register().WithoutPartName(WellKnownAppParts.DesignPatterns.Repository))
                .ApplyRegistrations();

            base.ConfigureContainer();

        }

        protected override IModuleCatalog GetModuleCatalog()
        {
            ModuleCatalog catalog = new ModuleCatalog();

            catalog.AddModule(typeof (Modules.Main.Module));

            return catalog;
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            RegionAdapterMappings mappings = base.ConfigureRegionAdapterMappings();

            mappings.RegisterMapping(typeof(Ribbon), Container.Resolve<RibbonRegionAdapter>());
            mappings.RegisterMapping(typeof(StackPanel), Container.Resolve<StackPanelRegionAdapter>());

            return mappings;
        }

        #endregion
    }
}
