﻿namespace Hydra.Import.Test
{
    using System;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using Hydra.Nh.Mappings;
    using Hydra.TestTools;
    using Microsoft.Practices.Unity;
    using NHibernate;
    using NHibernate.Cfg;
    using NHibernate.Tool.hbm2ddl;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ContainerDataAttribute : AutoDataAttribute
    {
        private static readonly IUnityContainer Container = new UnityContainer()
                    .RegisterType<ISessionFactory>(
                        new ContainerControlledLifetimeManager(),
                        new InjectionFactory(BuildSessionFactory))
                    .RegisterType<ISession>(new InjectionFactory(BuildSession))
                    .RegisterType<IStatelessSession>(new InjectionFactory(BuildStatelessSession));

        public ContainerDataAttribute()
            : base(new Fixture().Customize(
                new ContainerCustomization(Container)))
        {
        }

        private static object BuildSessionFactory(IUnityContainer container)
        {
            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.InMemory())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ResourceItemMap>())
                .ExposeConfiguration(cfg => BuildSchema(container, cfg))
                .BuildSessionFactory();
        }

        private static object BuildStatelessSession(IUnityContainer container)
        {
            ISessionFactory sessionFactory = container.Resolve<ISessionFactory>();

            IStatelessSession session = sessionFactory.OpenStatelessSession();

            Configuration configuration = container.Resolve<Configuration>();

            new SchemaExport(configuration).Execute(true, true, false, session.Connection, null);

            return session;
        }

        private static object BuildSession(IUnityContainer container)
        {
            ISessionFactory sessionFactory = container.Resolve<ISessionFactory>();

            ISession session = sessionFactory.OpenSession();

            Configuration configuration = container.Resolve<Configuration>();

            new SchemaExport(configuration).Execute(true, true, false, session.Connection, null);

            return session;
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