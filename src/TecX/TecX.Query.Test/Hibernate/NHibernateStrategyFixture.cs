﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

using FluentNHibernate.Mapping;

using NHibernate.Hql.Ast;
using NHibernate.Linq.Functions;
using NHibernate.Linq.Visitors;
using NHibernate.Type;

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
    using TecX.Query.Strategies;

    using Xunit;

    using Expression = System.Linq.Expressions.Expression;

    public class NHibernateStrategyFixture : IDisposable
    {
        private const string DbFile = "my.db";

        private readonly ISessionFactory sessionFactory;
        private readonly PDPrincipal muster;

        public NHibernateStrategyFixture()
        {
            log4net.Config.XmlConfigurator.Configure();

            sessionFactory = Fluently.Configure()
                                     .Database(SQLiteConfiguration.Standard.UsingFile(DbFile))
                                     .Mappings(map => map.FluentMappings.AddFromAssemblyOf<PrincipalMap>())
                                     .ExposeConfiguration(config =>
                                         {
                                             if (File.Exists(DbFile))
                                                 File.Delete(DbFile);

                                             // TODO weberse 2013-07-14 have a look at how Fetch and FetchMany are implemented and see wether there is a way to override them
                                             config.LinqToHqlGeneratorsRegistry<MyRegistry>();

                                             config.DataBaseIntegration(x =>
                                                 {
                                                     // very useful for debugging
                                                     x.AutoCommentSql = true;
                                                     x.LogFormattedSql = true;
                                                     //x.LogSqlInConsole = true;
                                                 });

                                             new SchemaExport(config)
                                                 .Create(false, true);
                                         })
                                     .BuildSessionFactory();

            using (ISession session = sessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    //PDPrincipal muster = new PDPrincipal { PrincipalName = "Muster AG" };
                    this.muster = new PDPrincipal { PrincipalName = "Muster AG" };
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

                    session.SaveOrUpdate(b1);
                    session.SaveOrUpdate(b2);
                    session.SaveOrUpdate(b3);
                    session.SaveOrUpdate(b4);
                    session.SaveOrUpdate(b5);

                    session.SaveOrUpdate(f1);
                    session.SaveOrUpdate(f2);
                    session.SaveOrUpdate(f3);
                    session.SaveOrUpdate(f4);
                    session.SaveOrUpdate(f5);

                    transaction.Commit();
                }
            }
        }

        [Fact]
        public void Should()
        {
            // if you don't close the first session nhibernate will perform a Linq2Object in-memory query and that will never filter out the items in the
            // sub-collections
            using (ISession session = sessionFactory.OpenSession())
            {
                //IFilter filter = session.EnableFilter(typeof(DescriptionFilter).Name).SetParameter(DescriptionFilter.Description, "B1");
                session.EnableFilter(typeof(PrincipalFilter).Name).SetParameter(BarMap.ForeignKeyColumns.Principal, this.muster.PDO_ID);

                IQueryable<Foo> nhibQuery = session.Query<Foo>();
                //IQueryable<Foo> nhibQuery = session.Query<Foo>().FetchMany(f => f.Bars).ThenFetch(bar => bar.Principal);

                //IQueryable<Foo> query = nhibQuery.Intercept(clientInfo: new ClientInfo { Principal = muster });
                IQueryable<Foo> query = nhibQuery;

                //query = query.FetchMany(f => f.Bars).ThenFetch(bar => bar.Principal);
                query = query.FetchMany(f => f.Bars).Where(bar => bar.Description.StartsWith("B"));

                //query = query.Where(f => f.Description.MyMethod("1"));

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

        public void Dispose()
        {
            this.sessionFactory.Dispose();
        }
    }

    public class DynamicFilter<T> : FilterDefinition
        where T : class
    {
        public DynamicFilter()
        {
            string name = typeof(T).FullName + "_DynamicFilter";

            WithName(name);

            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (PropertyInfo property in properties)
            {
                IType type = NHibernateUtil.GuessType(property.PropertyType);

                AddParameter(property.Name, type);
            }
        }
    }

    public static class Extensions
    {
        public static INhFetchRequest<TQueried, TFetch> Where<TQueried, TFetch>(
            this INhFetchRequest<TQueried, TFetch> query,
            Expression<Func<TFetch, bool>> whereClause)
        {
            var methodInfo = ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TQueried), typeof(TFetch));

            return CreateFluentFetchRequest<TQueried, TFetch>(methodInfo, query, whereClause);
        }

        private static INhFetchRequest<TOriginating, TRelated> CreateFluentFetchRequest<TOriginating, TRelated>(
            MethodInfo currentFetchMethod,
            IQueryable<TOriginating> query,
            LambdaExpression relatedObjectSelector)
        {
            var queryProvider = query.Provider;
            var callExpression = Expression.Call(currentFetchMethod, query.Expression, relatedObjectSelector);
            return new NhFetchRequest<TOriginating, TRelated>(queryProvider, callExpression);
        }
    }

    public class MyMethodGenerator : BaseHqlGeneratorForMethod
    {
        public override HqlTreeNode BuildHql(
            MethodInfo method,
            Expression targetObject,
            ReadOnlyCollection<Expression> arguments,
            HqlTreeBuilder treeBuilder,
            IHqlExpressionVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }

    public class MyGenerator : IRuntimeMethodHqlGenerator
    {
        public bool SupportsMethod(MethodInfo method)
        {
            if (method.Name.StartsWith("WithCondition", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            //if (string.Equals(method.Name, "MyMethod"))
            //{
            //    return true;
            //}

            return false;
        }

        public IHqlGeneratorForMethod GetMethodGenerator(MethodInfo method)
        {
            return new MyMethodGenerator();
        }
    }

    public class MyRegistry : DefaultLinqToHqlGeneratorsRegistry
    {
        public MyRegistry()
            : base()
        {
            this.RegisterGenerator(new MyGenerator());
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
