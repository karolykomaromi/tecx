using System.Windows;
using System.Windows.Controls;
using Infrastructure.Client;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;

namespace Modular
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            Shell shell = this.Container.Resolve<Shell>();

            return shell;
        }

        protected override void InitializeShell()
        {
            Shell shell = (Shell)this.Shell;
            Application.Current.RootVisual = shell;
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            RegionAdapterMappings mappings = base.ConfigureRegionAdapterMappings();

            mappings.RegisterMapping(typeof(StackPanel), this.Container.Resolve<StackPanelRegionAdapter>());

            return mappings;
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            this.Container.RegisterInstance(Deployment.Current.Dispatcher);
        }

        protected override void ConfigureModuleCatalog()
        {
            this.ModuleCatalog.AddModule(new ModuleInfo("Search", typeof(Search.Module).AssemblyQualifiedName));
        }
    }
}
