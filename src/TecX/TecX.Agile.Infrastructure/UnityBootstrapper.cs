using System;
using System.Collections.Generic;

using Caliburn.Micro;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.Unity;

using TecX.Agile.Infrastructure.Modularization;
using TecX.Common;
using TecX.Unity.Configuration;

namespace TecX.Agile.Infrastructure
{
    public class UnityBootstrapper<T> : Bootstrapper<T>
    {
        protected IModuleCatalog ModuleCatalog { get; set; }

        protected IUnityContainer Container { get; set; }

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
}