namespace Modular
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
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
    using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
    using Microsoft.Practices.EnterpriseLibrary.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
    using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
    using Microsoft.Practices.EnterpriseLibrary.Logging.Service;
    using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
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

        protected override ILoggerFacade CreateLogger()
        {
            // manually assembling a remote tracelistener is way too much effort :(
            TraceListener remote = new RemoteServiceTraceListener(
                true,
                new LoggingServiceFactory { EndpointConfigurationName = "BasicHttpBinding_ILoggingService" },
                new RecurringWorkScheduler(TimeSpan.FromMinutes(1)),
                new LogEntryMessageStore("remote", 100, 0),
                new AsyncTracingErrorReporter(),
                new NetworkStatus());

            var debug = new LogSource("Debug", new[] { remote }, SourceLevels.All);
            var info = new LogSource("Info", new[] { remote }, SourceLevels.All);
            var warn = new LogSource("Warn", new[] { remote }, SourceLevels.All);
            var exception = new LogSource("Exception", new[] { remote }, SourceLevels.All);
            
            var traceSources = new Dictionary<string, LogSource>
                {
                    { "Debug", debug },
                    { "Info", info },
                    { "Warn", warn },
                    { "Exception", exception }
                };

            var general = new LogSource("general", new[] { remote }, SourceLevels.All);
            var notProcessed = new LogSource("general", new[] { remote }, SourceLevels.All);
            var errors = new LogSource("general", new[] { remote }, SourceLevels.All);

            LogWriter logWriter = new LogWriterImpl(new ILogFilter[0], traceSources, general, notProcessed, errors, "general", true, true);
            ILoggerFacade entLibLogger = new EnterpriseLibraryLogger(logWriter);
            
            CompositeLogger composite = new CompositeLogger(entLibLogger, new DebugLogger());

            return composite;
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            this.Container.AddNewExtension<RegionExtension>();

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
