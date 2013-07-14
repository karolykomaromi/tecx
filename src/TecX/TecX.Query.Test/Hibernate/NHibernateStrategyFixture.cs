namespace TecX.Query.Test.Hibernate
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;

    using NHibernate;
    using NHibernate.Cfg;
    using NHibernate.Linq;
    using NHibernate.Tool.hbm2ddl;


    using TecX.Query.PD;
    using TecX.Query.Simulation;
    using TecX.Query.Strategies;

    using Xunit;

    using Expression = System.Linq.Expressions.Expression;

    public class NHibernateStrategyFixture
    {
        private const string DbFile = "my.db";

        [Fact]
        public void Should()
        {
            ISessionFactory sessionFactory = Fluently.Configure()
                                                .Database(SQLiteConfiguration.Standard.UsingFile(DbFile))
                                                .Mappings(map => map.FluentMappings.AddFromAssemblyOf<PrincipalMap>())
                                                .ExposeConfiguration(config =>
                                                    {
                                                        if (File.Exists(DbFile))
                                                            File.Delete(DbFile);

                                                        // TODO weberse 2013-07-14 have a look at how Fetch and FetchMany are implemented and see wether there is a way to override them
                                                        //config.LinqToHqlGeneratorsRegistry<MyLinqToHqlGeneratorsRegistry>();

                                                        new SchemaExport(config)
                                                          .Create(false, true);
                                                    })
                                                .BuildSessionFactory();

            using (ISession session = sessionFactory.OpenSession())
            {
                PDPrincipal muster = null;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    //PDPrincipal muster = new PDPrincipal { PrincipalName = "Muster AG" };
                    muster = new PDPrincipal { PrincipalName = "Muster AG" };
                    PDPrincipal kommerz = new PDPrincipal { PrincipalName = "Kommerz GmbH" };

                    Bar b1 = new Bar { Description = "B1", Principal = kommerz };
                    Bar b2 = new Bar { Description = "B2", Principal = kommerz };
                    Bar b3 = new Bar { Description = "B3", Principal = kommerz };
                    Bar b4 = new Bar { Description = "B4", Principal = muster };
                    Bar b5 = new Bar { Description = "B5", Principal = muster };

                    Foo f1 = new Foo { Description = "F1", Principal = muster };
                    f1.Bars.Add(b1);
                    f1.Bars.Add(b4);

                    Foo f2 = new Foo { Description = "F2", Principal = muster };
                    f2.Bars.Add(b5);

                    Foo f3 = new Foo { Description = "F3", Principal = muster };
                    Foo f4 = new Foo { Description = "F4", Principal = kommerz };
                    f4.Bars.Add(b2);
                    f4.Bars.Add(b3);

                    Foo f5 = new Foo { Description = "F5", Principal = kommerz };

                    session.SaveOrUpdate(muster);
                    session.SaveOrUpdate(kommerz);

                    session.SaveOrUpdate(f1);
                    session.SaveOrUpdate(f2);
                    session.SaveOrUpdate(f3);
                    session.SaveOrUpdate(f4);
                    session.SaveOrUpdate(f5);

                    transaction.Commit();
                }

                session.EnableFilter(typeof(DescriptionFilter).Name).SetParameter(DescriptionFilter.Description.ToLower(), "B1");

                IQueryable<Foo> nhibQuery = session.Query<Foo>();

                IQueryable<Foo> query = nhibQuery.Intercept(clientInfo: new ClientInfo { Principal = muster });

                query = query.FetchMany(f => f.Bars);

                foreach (Foo foo in query)
                {
                    Console.WriteLine(foo.Description);

                    foreach (Bar bar in foo.Bars)
                    {
                        Console.WriteLine("\t" + bar.Description);
                    }
                }
            }
        }
    }

    public class NHibernateStrategy : ExpressionManipulationStrategy
    {
        public override Expression Process(Expression expression, Type elementType)
        {
            return new EvalVisitor().Visit(expression);
        }
    }

    public class EvalVisitor : ExpressionVisitor
    {
        public override Expression Visit(Expression node)
        {
            using (new DebugIndentor())
            {
                Debug.WriteLine(node);
                return base.Visit(node);
            }
        }
    }

    public class DebugIndentor : IDisposable
    {
        public DebugIndentor()
        {
            Debug.Indent();
        }

        public void Dispose()
        {
            Debug.Unindent();
        }
    }
}
