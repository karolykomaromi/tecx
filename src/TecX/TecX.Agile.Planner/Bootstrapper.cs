using System.Windows;
using System.Windows.Controls;

using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;

using TecX.Agile.ChangeTracking;
using TecX.Agile.Infrastructure.Services;
using TecX.Agile.Modules.Main;
using TecX.Agile.Modules.Main.Services;
using TecX.Agile.Peer;
using TecX.Agile.Remote;
using TecX.Common.Event.Unity;
using TecX.Prism.Regions;

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

            Container.AddNewExtension<EventAggregatorContainerExtension>()
                //.RegisterType<IRepository, XmlRepository>()
                .RegisterType<IRemoteUI, WcfPeerRemoteUI>()
                .RegisterType<IPeerClient, PeerClient>()
                .RegisterType<IChangeTracker, ChangeTracker>();

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

            ModuleInfo main = new ModuleInfo(Module.ModuleName, typeof (Module).AssemblyQualifiedName);

            ModuleCatalog.AddModule(main);

            ModuleInfo gestures = new ModuleInfo(Modules.Gestures.Module.ModuleName,
                                                 typeof (Modules.Gestures.Module).AssemblyQualifiedName);

            ModuleCatalog.AddModule(gestures);

            ModuleInfo systemInfo = new ModuleInfo(Modules.SysInfo.Module.ModuleName,
                                                   typeof (Modules.SysInfo.Module).AssemblyQualifiedName);

            ModuleCatalog.AddModule(systemInfo);
        }


        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            RegionAdapterMappings mappings = base.ConfigureRegionAdapterMappings();

            mappings.RegisterMapping(typeof (StackPanel), Container.Resolve<StackPanelRegionAdapter>());
            mappings.RegisterMapping(typeof (Grid), Container.Resolve<GridRegionAdapter>());

            return mappings;
        }

        #endregion
    }
}