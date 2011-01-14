using System.Windows;
using System.Windows.Controls;

using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;

using TecX.Agile.Infrastructure.Services;
using TecX.Agile.Modules.Main.Services;
using TecX.Agile.Peer;
using TecX.Agile.ChangeTracking;
using TecX.Agile.Remote;
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
                .Include(If.Is<ChangeTracker>(), Then.Register().As<IChangeTracker>())
                .ApplyRegistrations();

            Container.RegisterType<IShowThings, ShowThingsService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IShowText, ShowTextService>(new ContainerControlledLifetimeManager());

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

            ModuleInfo gestures = new ModuleInfo(Modules.Gestures.Module.ModuleName, typeof(Modules.Gestures.Module).AssemblyQualifiedName);

            ModuleCatalog.AddModule(gestures);
        }


        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            RegionAdapterMappings mappings = base.ConfigureRegionAdapterMappings();

            mappings.RegisterMapping(typeof(StackPanel), Container.Resolve<StackPanelRegionAdapter>());
            mappings.RegisterMapping(typeof(Grid), Container.Resolve<GridRegionAdapter>());

            return mappings;
        }

        #endregion
    }
}
