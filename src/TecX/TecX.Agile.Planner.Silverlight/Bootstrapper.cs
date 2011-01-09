using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.UnityExtensions;

using TecX.Agile.ChangeTracking;
using TecX.Agile.Remote;
using TecX.Common.Event.Unity;
using TecX.Prism.Regions;

namespace TecX.Agile.Planner
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            Shell shell = Container.Resolve<Shell>();

            Application.Current.RootVisual = shell;

            return shell;
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.AddNewExtension<EventAggregatorContainerExtension>();

            Container.RegisterInstance(Deployment.Current.Dispatcher,
                                                   new ContainerControlledLifetimeManager());

            Container.RegisterType<IChangeTracker, ChangeTracker>();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            IModuleCatalog catalog = new ModuleCatalog();

            return catalog;
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            RegionAdapterMappings mappings = base.ConfigureRegionAdapterMappings();

            mappings.RegisterMapping(typeof(StackPanel), Container.Resolve<StackPanelRegionAdapter>());
            mappings.RegisterMapping(typeof(Grid), Container.Resolve<GridRegionAdapter>());

            return mappings;
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            ModuleInfo main = new ModuleInfo(Modules.Main.Module.ModuleName, typeof(Modules.Main.Module).AssemblyQualifiedName);

            ModuleCatalog.AddModule(main);
        }
    }
}
