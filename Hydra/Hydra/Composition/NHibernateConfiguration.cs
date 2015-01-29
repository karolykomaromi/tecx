namespace Hydra.Composition
{
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using Hydra.Infrastructure;
    using Hydra.Infrastructure.I18n;
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
                    .Mappings(m =>
                        {
                            m.FluentMappings.AddFromAssemblyOf<ResourceItemMap>();
                            m.FluentMappings.Conventions.AddFromAssemblyOf<MultiLanguageStringUserTypeConvention>();
                        })
                    .BuildSessionFactory()));

            this.Container.RegisterType<ISession>(
                new HierarchicalLifetimeManager(),
                new InjectionFactory(c => new LazySessionWithAutoTx(() => c.Resolve<ISessionFactory>().OpenSession())));

            this.Container.RegisterType<IStatelessSession>(
                new HierarchicalLifetimeManager(),
                new InjectionFactory(c => new LazyStatelessSessionWithAutoTx(() => c.Resolve<ISessionFactory>().OpenStatelessSession())));
        }
    }
}