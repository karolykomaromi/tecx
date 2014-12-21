namespace Hydra.Configuration
{
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using Hydra.Nh;
    using Hydra.Nh.Mappings;
    using Microsoft.Practices.Unity;
    using NHibernate;

    public class NHibernateConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Container.RegisterType<ISessionFactory>(
                new ContainerControlledLifetimeManager(),
                new InjectionFactory(_ => (object)Fluently.Configure()
                    .Database(MySQLConfiguration.Standard.ConnectionString(c => c.FromConnectionStringWithKey("mysql")))
                    .Mappings(m => m.FluentMappings.Add<ResourceItemMap>())
                    .BuildSessionFactory()));

            this.Container.RegisterType<ISession>(
                new HierarchicalLifetimeManager(),
                new InjectionFactory(c => new LazySessionWithAutoTx(() => c.Resolve<ISessionFactory>().OpenSession())));
        }
    }
}