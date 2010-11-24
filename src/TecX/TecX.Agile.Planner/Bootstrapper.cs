using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using Microsoft.Windows.Controls.Ribbon;

using TecX.Agile.Peer;
using TecX.Agile.ViewModel.Remote;
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
                .Include(If.Is<WcfPeerRemoteUI>(), Then.Register().As<IRemoteUI>())
                .Include(If.Is<PeerClient>(), Then.Register().As<IPeerClient>())
                .ApplyRegistrations();

            base.ConfigureContainer();

        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            ModuleCatalog catalog = new ModuleCatalog();

            return catalog;
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            ModuleInfo main = new ModuleInfo(Modules.Main.Module.ModuleName, typeof(Modules.Main.Module).AssemblyQualifiedName);

            ModuleCatalog.AddModule(main);
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
