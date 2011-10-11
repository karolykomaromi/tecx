using System.Windows;
using System.Windows.Controls;

using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;

using TecX.Prism.Regions;
using TecX.Unity.Configuration;
using TecX.Unity.Configuration.Conventions;

namespace TecX.Agile
{
    using TecX.Agile.Registration;

    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            Shell shell = Container.Resolve<Shell>();

            return shell;
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Window)this.Shell;
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Registry registry = new EntLibRegistry();

            registry.Scan(x =>
                {
                    x.With(new FindRegistriesConvention());
                    x.AssembliesFromApplicationBaseDirectory();
                });

            Container.AddExtension(registry);
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            var catalog = ModuleCatalog;

            //TODO weberse 2011-10-11 configure catalog
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            ModuleCatalog catalog = new ModuleCatalog();

            return catalog;
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            RegionAdapterMappings mappings = base.ConfigureRegionAdapterMappings();

            mappings.RegisterMapping(typeof(StackPanel), Container.Resolve<StackPanelRegionAdapter>());
            mappings.RegisterMapping(typeof(Grid), Container.Resolve<GridRegionAdapter>());

            return mappings;
        }
    }
}
