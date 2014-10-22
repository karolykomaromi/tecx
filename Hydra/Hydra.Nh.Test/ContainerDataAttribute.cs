using NHibernate.Cfg;

namespace Hydra.Nh.Test
{
    using System;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using Hydra.Nh.Mappings;
    using Hydra.TestTools;
    using Microsoft.Practices.Unity;
    using NHibernate;
    using NHibernate.Tool.hbm2ddl;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ContainerDataAttribute : AutoDataAttribute
    {
        private static readonly IUnityContainer Container = new UnityContainer().RegisterType<ISessionFactory>(
                new ContainerControlledLifetimeManager(),
                new InjectionFactory(_ => Fluently.Configure()
                    .Database(MySQLConfiguration.Standard.ConnectionString(c => c.FromConnectionStringWithKey("mysql")))
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ResourceItemMap>())
                    .ExposeConfiguration(BuildSchema)
                    .BuildSessionFactory())).RegisterType<ISession>(
                new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<ISessionFactory>().OpenSession()));

        public ContainerDataAttribute()
            : base(new Fixture().Customize(
                new ContainerCustomization(ContainerDataAttribute.Container)))
        {
        }

        private static void BuildSchema(Configuration configuration)
        {
            SchemaUpdate update = new SchemaUpdate(configuration);

            update.Execute(false, true);

            if (update.Exceptions != null && update.Exceptions.Count > 0)
            {
                throw new AggregateException(update.Exceptions);
            }
        }
    }
}
