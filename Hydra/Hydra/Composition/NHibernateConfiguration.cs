namespace Hydra.Composition
{
    using System;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using Hydra.Infrastructure;
    using Hydra.Infrastructure.I18n;
    using Microsoft.Practices.Unity;
    using NHibernate;
    using NHibernate.Cfg;
    using NHibernate.Tool.hbm2ddl;

    public class NHibernateConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Container.RegisterType<ISessionFactory>(
                new ContainerControlledLifetimeManager(),
                new InjectionFactory(_ => (object)Fluently.Configure()
                    .Database(MySQLConfiguration.Standard.ConnectionString(c => c.FromConnectionStringWithKey("mysql")))
                    ////.Database(SQLiteConfiguration.Standard.InMemory())
                    .Mappings(m =>
                        {
                            m.FluentMappings.AddFromAssemblyOf<ResourceItemMap>();
                            m.FluentMappings.Conventions.AddFromAssemblyOf<PolyglotStringUserTypeConvention>();
                        })
                    ////.ExposeConfiguration(cfg => BuildSchema(this.Container, cfg))
                    .BuildSessionFactory()));

            this.Container.RegisterType<ISession>(
                new HierarchicalLifetimeManager(),
                new InjectionFactory(c => new LazySessionWithAutoTx(() => c.Resolve<ISessionFactory>().OpenSession())));

            this.Container.RegisterType<IStatelessSession>(
                new HierarchicalLifetimeManager(),
                new InjectionFactory(c => new LazyStatelessSessionWithAutoTx(() => c.Resolve<ISessionFactory>().OpenStatelessSession())));
        }

        private static void BuildSchema(IUnityContainer container, Configuration configuration)
        {
            SchemaUpdate update = new SchemaUpdate(configuration);

            update.Execute(false, true);

            container.RegisterInstance(configuration);

            // TODO weberse 2014-12-22 NHibernate can use alternative factories to create its objects
            ////NHibernate.Cfg.Environment.BytecodeProvider

            if (update.Exceptions != null && update.Exceptions.Count > 0)
            {
                throw new AggregateException(update.Exceptions);
            }
        }
    }
}