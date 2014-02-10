namespace Modular
{
    using System.Collections;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;
    using System.Windows.Threading;
    using AutoMapper;
    using Infrastructure;
    using Infrastructure.Events;
    using Infrastructure.I18n;
    using Infrastructure.Modularity;
    using Infrastructure.Options;
    using Infrastructure.UnityExtensions;
    using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
    using Microsoft.Practices.Prism.Logging;
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

        protected override ILoggerFacade CreateLogger()
        {
            return new DebugLogger();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            this.Container.AddNewExtension<RegionExtension>();

            this.Container.RegisterInstance(Deployment.Current.Dispatcher);
            this.Container.AddNewExtension<EventAggregatorExtension>();
            this.Container.RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager());

            string xaml;
            using (Stream s = this.GetType().Assembly.GetManifestResourceStream("Modular.CacheConfig.xaml"))
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    xaml = sr.ReadToEnd();
                }
            }

            IDictionary configDictionary = (IDictionary)XamlReader.Load(xaml);
            DictionaryConfigurationSource configSource = DictionaryConfigurationSource.FromDictionary(configDictionary);
            EnterpriseLibraryContainer.ConfigureContainer(new UnityContainerConfigurator(this.Container), configSource);

            this.Container.AddNewExtension<ResourceManagerExtension>();
            this.Container.RegisterType<IResourceManager, CompositeResourceManager>(Constants.AppWideResources, new ContainerControlledLifetimeManager(), new InjectionConstructor());
            this.Container.RegisterType<IResourceManager, CachingResourceManager>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(new ResolvedParameter<IResourceManager>(Constants.AppWideResources), typeof(IEventAggregator), typeof(ObjectCache)));

            this.Container.RegisterType<ModuleResourcesInitializer>(new InjectionConstructor(Application.Current.Resources));
            this.Container.RegisterType<ResourceManagerInitializer>(new InjectionConstructor(new ResolvedParameter<CompositeResourceManager>(Constants.AppWideResources)));
            this.Container.RegisterType<Infrastructure.Modularity.IModuleInitializer, DefaultInitializer>();

            this.Container.RegisterInstance<IMappingEngine>(Mapper.Engine);

            this.Container.RegisterType<IListViewService, ListViewServiceClient>(Constants.Client, new InjectionConstructor());
            this.Container.RegisterType<IListViewService, DispatchingListViewServiceClient>(
                new InjectionConstructor(new ResolvedParameter<IListViewService>(Constants.Client), typeof(Dispatcher)));
            
            this.Container.RegisterType<IOptions, CompositeOptions>(new ContainerControlledLifetimeManager());
        }

        protected override void ConfigureModuleCatalog()
        {
            this.ModuleCatalog.AddModule(new ModuleInfo(Main.Module.Name, typeof(Main.Module).AssemblyQualifiedName));
            this.ModuleCatalog.AddModule(new ModuleInfo(Search.Module.Name, typeof(Search.Module).AssemblyQualifiedName));
            this.ModuleCatalog.AddModule(new ModuleInfo(Recipe.Module.Name, typeof(Recipe.Module).AssemblyQualifiedName));
        }

        private static class Constants
        {
            /// <summary>
            /// appWideResources
            /// </summary>
            public const string AppWideResources = "appWideResources";

            /// <summary>
            /// Client
            /// </summary>
            public const string Client = "client";
        }
    }
}
