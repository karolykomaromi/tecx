namespace Modular
{
    using System.Windows;
    using System.Windows.Controls;
    using Infrastructure;
    using Infrastructure.Commands;
    using Infrastructure.Events;
    using Microsoft.Practices.Prism.Modularity;
    using Microsoft.Practices.Prism.Regions;
    using Microsoft.Practices.Prism.UnityExtensions;
    using Microsoft.Practices.Unity;

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
            this.Container.AddNewExtension<CommandManagerExtension>();
            this.Container.RegisterType<ICommandManager, CommandManager>(new ContainerControlledLifetimeManager());
            this.Container.AddNewExtension<EventAggregatorExtension>();
            this.Container.RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager());
        }

        protected override void ConfigureModuleCatalog()
        {
            this.ModuleCatalog.AddModule(new ModuleInfo("Search", typeof(Search.Module).AssemblyQualifiedName));
            this.ModuleCatalog.AddModule(new ModuleInfo("Details", typeof(Details.Module).AssemblyQualifiedName));
        }
    }
}
