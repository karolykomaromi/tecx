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
        }

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

            builder.Scan(x =>
            {
                x.LookForConfigBuilders();
                x.AssembliesFromApplicationBaseDirectory();
            });

            Container.AddExtension(builder);
        }

        protected virtual void ConfigureServiceLocator()
        {
            UnityServiceLocator locator = new UnityServiceLocator(Container);

            EnterpriseLibraryContainer.Current = locator;
        }

        protected sealed override IEnumerable<object> GetAllInstances(Type service)
        {
            Guard.AssertNotNull(service, "service");

            var instances = Container.ResolveAll(service);

            return instances;
        }

        protected sealed override object GetInstance(Type service, string key)
        {
            Guard.AssertNotNull(service, "service");

            var instance = Container.Resolve(service, key);

            return instance;
        }

        protected sealed override void BuildUp(object instance)
        {
            Guard.AssertNotNull(instance, "instance");

            var builtUp = Container.BuildUp(instance.GetType(), instance);
        }
    }

    public class ModuleCatalog : IModuleCatalog
    {
    }

    public interface IModuleCatalog
    {
    }
}