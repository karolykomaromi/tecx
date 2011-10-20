using System;
using System.Collections.Generic;

using Caliburn.Micro;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.Unity;

using TecX.Common;
using TecX.Unity.Configuration;

namespace TecX.Agile.Infrastructure
{
    public class UnityBootstrapper<T> : Bootstrapper<T>
    {
        protected IModuleCatalog ModuleCatalog { get; set; }

        protected IUnityContainer Container { get; set; }

        protected sealed override void Configure()
        {
            base.Configure();

            LogManager.GetLog = CreateLogger();

            ModuleCatalog = CreateModuleCatalog();

            ConfigureModuleCatalog();

            Container = CreateContainer();

            ConfigureContainer();

            ConfigureServiceLocator();

            InitializeModules();
        }

        #region Initialization

        protected virtual Func<Type, ILog> CreateLogger()
        {
            return type => new DebugLogger(type);
        }

        protected virtual IModuleCatalog CreateModuleCatalog()
        {
            return new ModuleCatalog();
        }

        protected virtual void ConfigureModuleCatalog()
        {
        }

        protected virtual IUnityContainer CreateContainer()
        {
            return new UnityContainer();
        }

        protected virtual void ConfigureContainer()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();

            builder.Scan(
                x =>
                {
                    x.LookForConfigBuilders();
                    x.AssembliesFromApplicationBaseDirectory();
                });

            Container.AddExtension(builder);

            Container.RegisterInstance(ModuleCatalog);

            Container.RegisterType<IModuleManager, ModuleManager>(new ContainerControlledLifetimeManager());
        }

        protected virtual void ConfigureServiceLocator()
        {
            UnityServiceLocator locator = new UnityServiceLocator(Container);

            EnterpriseLibraryContainer.Current = locator;
        }

        protected virtual void InitializeModules()
        {
            IModuleManager manager;

            try
            {
                manager = this.Container.Resolve<IModuleManager>();
            }
            catch (ResolutionFailedException ex)
            {
                if (ex.Message.Contains("IModuleCatalog"))
                {
                    throw new InvalidOperationException();
                }

                throw;
            }

            manager.Run();
        }

        #endregion

        #region Overrides of Bootstrapper<T>

        protected override sealed IEnumerable<object> GetAllInstances(Type service)
        {
            Guard.AssertNotNull(service, "service");

            var instances = Container.ResolveAll(service);

            return instances;
        }

        protected override sealed object GetInstance(Type service, string key)
        {
            Guard.AssertNotNull(service, "service");

            var instance = Container.Resolve(service, key);

            return instance;
        }

        protected override sealed void BuildUp(object instance)
        {
            Guard.AssertNotNull(instance, "instance");

            var builtUp = Container.BuildUp(instance.GetType(), instance);
        }

        #endregion Overrides of Bootstrapper<T>
    }

    public interface IModuleManager
    {
        void Run();
    }

    public class ModuleManager : IModuleManager
    {
        private readonly IModuleCatalog _catalog;

        private readonly IUnityContainer _container;

        public ModuleManager(IModuleCatalog catalog, IUnityContainer container)
        {
            Guard.AssertNotNull(catalog, "catalog");
            Guard.AssertNotNull(container, "container");

            _catalog = catalog;
            _container = container;
        }

        public void Run()
        {
            foreach (var moduleInfo in _catalog.Modules)
            {
                Type moduleType = Type.GetType(moduleInfo.ModuleType);

                IModule module = (IModule)_container.Resolve(moduleType, moduleInfo.Name);

                module.Initialize();
            }
        }
    }

    public class ModuleCatalog : IModuleCatalog
    {
        private readonly List<ModuleInfo> _modules;

        public ModuleCatalog()
        {
            _modules = new List<ModuleInfo>();
        }

        public IEnumerable<ModuleInfo> Modules
        {
            get
            {
                return _modules;
            }
        }

        public void AddModule(Type moduleType)
        {
            Guard.AssertNotNull(moduleType, "moduleType");

            ModuleInfo module = new ModuleInfo
                {
                    Name = moduleType.Name,
                    ModuleType = moduleType.AssemblyQualifiedName
                };

            AddModule(module);
        }

        public void AddModule(ModuleInfo module)
        {
            Guard.AssertNotNull(module, "module");

            _modules.Add(module);
        }
    }

    public interface IModuleCatalog
    {
        IEnumerable<ModuleInfo> Modules { get; }

        void AddModule(ModuleInfo module);
    }

    public class ModuleInfo
    {
        public string Name { get; set; }

        public string ModuleType { get; set; }
    }
}