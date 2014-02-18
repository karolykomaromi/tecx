using Infrastructure.UnityExtensions.Injection;

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
    using Infrastructure.Logging;
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
        public IModuleTracker ModuleTracker { get; protected set; }

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

        protected override IUnityContainer CreateContainer()
        {
            return this.Container;
        }

        protected override ILoggerFacade CreateLogger()
        {
            this.Container = new UnityContainer();

            // The logger is about the first thing created by the bootstrapper. That means the default container is not yet
            // created and we can't use it in this step. So we create a throw-away container just for resolving the RemoteServiceTraceListener.
            // Maybe I will change that when I find out how to manually assemble the configuration.
            string xaml;
            using (Stream stream = this.GetType().Assembly.GetManifestResourceStream("Modular.LoggingConfiguration.xaml"))
            {
                using (TextReader reader = new StreamReader(stream))
                {
                    xaml = reader.ReadToEnd();
                }
            }

            IDictionary configDictionary = (IDictionary)XamlReader.Load(xaml);
            IConfigurationSource configSource = DictionaryConfigurationSource.FromDictionary(configDictionary);
            EnterpriseLibraryContainer.ConfigureContainer(new UnityContainerConfigurator(this.Container), configSource);

            ILoggerFacade entLibLogger = this.Container.Resolve<EnterpriseLibraryLogger>();

            CompositeLogger composite = new CompositeLogger(entLibLogger, new DebugLogger());

            return composite;
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            this.Container.AddNewExtension<RegionExtension>();
            this.Container.AddNewExtension<MapToRegistrationNamesExtension>();

            this.Container.RegisterInstance(Deployment.Current.Dispatcher);
            this.Container.AddNewExtension<EventAggregatorExtension>();
            this.RegisterTypeIfMissing(typeof(IEventAggregator), typeof(EventAggregator), true);
            this.RegisterTypeIfMissing(typeof(IModuleTracker), typeof(ModuleTracker), true);

            string xaml;
            using (Stream stream = this.GetType().Assembly.GetManifestResourceStream("Modular.CachingConfiguration.xaml"))
            {
                using (TextReader reader = new StreamReader(stream))
                {
                    xaml = reader.ReadToEnd();
                }
            }

            IDictionary configDictionary = (IDictionary)XamlReader.Load(xaml);
            IConfigurationSource configSource = DictionaryConfigurationSource.FromDictionary(configDictionary);
            EnterpriseLibraryContainer.ConfigureContainer(new UnityContainerConfigurator(this.Container), configSource);

            this.Container.RegisterType<IResourceManager, CompositeResourceManager>(Constants.AppWideResources, new ContainerControlledLifetimeManager(), new InjectionConstructor());
            this.Container.RegisterType<IResourceManager, CachingResourceManager>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(new ResolvedParameter<IResourceManager>(Constants.AppWideResources), typeof(IEventAggregator), typeof(ObjectCache), typeof(ILoggerFacade)));

            this.Container.RegisterType<ModuleResourcesInitializer>(new InjectionConstructor(Application.Current.Resources));
            this.Container.RegisterType<Infrastructure.Modularity.IModuleInitializer, DefaultInitializer>();

            this.Container.RegisterInstance<IMappingEngine>(Mapper.Engine);

            this.Container.RegisterType<IListViewService, ListViewServiceClient>(new InjectionConstructor(typeof(Dispatcher)));

            this.RegisterTypeIfMissing(typeof(IOptions), typeof(CompositeOptions), true);
        }

        protected override void ConfigureModuleCatalog()
        {
            this.ModuleCatalog.AddModule(new ModuleInfo(Main.Module.Name, typeof(Main.Module).AssemblyQualifiedName));
            this.ModuleCatalog.AddModule(new ModuleInfo(Search.Module.Name, typeof(Search.Module).AssemblyQualifiedName, Main.Module.Name));

            this.ModuleCatalog.AddModule(new ModuleInfo("Recipe", "Recipe.Module, Recipe.Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Search") { Ref = "Recipe.xap", InitializationMode = InitializationMode.WhenAvailable });
        }

        protected override void InitializeModules()
        {
            IModuleManager moduleManager = this.Container.Resolve<IModuleManager>(new ResolverOverride[0]);
            IModuleTracker tracker = this.Container.Resolve<IModuleTracker>();

            foreach (ModuleInfo module in this.ModuleCatalog.Modules)
            {
                tracker.Register(new ModuleTrackingState
                    {
                        ModuleName = module.ModuleName,
                        ExpectedInitializationMode = module.InitializationMode,
                        ConfiguredDependencies = module.Ref
                    });
            }

            moduleManager.LoadModuleCompleted += (s, e) => tracker.RecordModuleLoaded(e.ModuleInfo.ModuleName);
            moduleManager.ModuleDownloadProgressChanged += (s, e) => tracker.RecordModuleDownloading(e.ModuleInfo.ModuleName, e.BytesReceived, e.TotalBytesToReceive);

            moduleManager.Run();
        }

        private static class Constants
        {
            /// <summary>
            /// appWideResources
            /// </summary>
            public const string AppWideResources = "appWideResources";
        }
    }
}
