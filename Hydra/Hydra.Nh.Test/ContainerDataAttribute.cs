namespace Hydra.Nh.Test
{
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using Hydra.Nh.Mappings;
    using Hydra.TestTools;
    using Microsoft.Practices.Unity;
    using NHibernate;
    using NHibernate.Tool.hbm2ddl;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit;

    public class ContainerDataAttribute : AutoDataAttribute
    {
        private static readonly IUnityContainer Container = new UnityContainer().RegisterType<ISessionFactory>(
                new ContainerControlledLifetimeManager(),
                new InjectionFactory(_ => Fluently.Configure()
                    .Database(MySQLConfiguration.Standard.ConnectionString(c => c.FromConnectionStringWithKey("mysql")))
                    .Mappings(m => m.FluentMappings.Add<ResourceItemMap>())
                    .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                    .BuildSessionFactory())).RegisterType<ISession>(
                new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<ISessionFactory>().OpenSession()));

        public ContainerDataAttribute()
            : base(new Fixture().Customize(
                new ContainerCustomization(ContainerDataAttribute.Container)))
        {
        }
    }
}
