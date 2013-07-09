using System;
using System.Linq.Expressions;
using System.Reflection;

namespace TecX.Playground.QueryAbstractionLayer
{
    using System.Linq;

    using TecX.Playground.QueryAbstractionLayer.PD;
    using TecX.Playground.QueryAbstractionLayer.Simulation;

    using Xunit;

    public class QueryInterceptorFixture
    {
        [Fact]
        public void Should_UseFrameworkFilters_WhenWhereClauseExists()
        {
            IQueryable<Foo> query = new[]
                {
                    new Foo { Principal = new PDPrincipal { PDO_ID = 1 } }, 
                    new Foo { Principal = new PDPrincipal { PDO_ID = 2 } }, 
                    new Foo { Principal = new PDPrincipal { PDO_ID = 1337 } }
                }.AsQueryable();

            IQueryable<Foo> intercepted = new QueryInterceptor<Foo>(query, new PDIteratorOperator(), new ClientInfo { Principal = new PDPrincipal { PDO_ID = 1337 } }).Where(f => string.IsNullOrEmpty(f.Bar));

            Assert.Equal(1, intercepted.Count());
        }

        [Fact]
        public void Should_UseFrameworkFilters_WhenNoWhereClauseExists()
        {
            IQueryable<Foo> query = new[]
                {
                    new Foo { Principal = new PDPrincipal { PDO_ID = 1 } }, 
                    new Foo { Principal = new PDPrincipal { PDO_ID = 2 } }, 
                    new Foo { Principal = new PDPrincipal { PDO_ID = 1337 } }
                }.AsQueryable();

            IQueryable<Foo> intercepted = new QueryInterceptor<Foo>(query, new PDIteratorOperator(), new ClientInfo { Principal = new PDPrincipal { PDO_ID = 1337 } });

            Assert.Equal(1, intercepted.Count());
        }

        [Fact]
        public void Show_Usage()
        {
            ISession session = new SessionImpl();

            PDIteratorOperator pdOperator = new PDIteratorOperator();

            IQueryable<Foo> query = session.Query<Foo>(pdOperator);

            // ...
        }

        [Fact]
        public void Should()
        {
            IQueryable<Foo> items = new[]
                {
                    new Foo { Principal = new PDPrincipal { PDO_ID = 1 } }, 
                    new Foo { Principal = new PDPrincipal { PDO_ID = 2 } }, 
                    new Foo { Principal = new PDPrincipal { PDO_ID = 1337 } }
                }.AsQueryable();

            //MethodInfo countMethod = GetCountMethodInfo(typeof(Foo));

            //ParameterExpression p = Expression.Parameter(typeof(IQueryable<Foo>), "p");

            //MethodCallExpression callCount = Expression.Call(countMethod, p);

            //Expression<Func<IQueryable<Foo>, int>> count = Expression.Lambda<Func<IQueryable<Foo>, int>>(callCount, p);

            Expression<Func<IQueryable<Foo>, int>> count = x => x.Count();

            Expression<Func<IQueryable<Foo>, IQueryable<Foo>>> filter = x => x.Where(foo => foo.Principal.PDO_ID < 3);

            MethodCallExpression callCount = count.Body as MethodCallExpression;

            //Where(filter)

            //MethodInfo whereMethod = null;

            //ParameterExpression queryableParameter = count.Parameters[0];

            //Expression<Func<Foo, bool>> whereClause = foo => foo.Principal.PDO_ID < 3;

            //MethodCallExpression callWhere = Expression.Call(whereMethod, queryableParameter, whereClause);






            //ConstantExpression maxID = Expression.Constant((long)3, typeof(long));

            //ParameterExpression foo = Expression.Parameter(typeof (Foo), "foo");

            //MemberExpression pdoID = Expression.Property(Expression.Property(foo, "Principal"), "PDO_ID");

            //BinaryExpression lessThan = Expression.LessThan(pdoID, maxID);



            //Expression<Func<Foo, bool>> filter = foo => foo.Principal.PDO_ID < 3;

            //int c = count.Compile()(query);


            //Expression<Func<Foo, bool>> filter = Expression.Lambda<Func<Foo, bool>>(lessThan, foo);

            //Assert.Equal(2, query.Where(filter).Count());

            //Assert.Equal(3, c);
        }

        private static MethodInfo GetCountMethodInfo(Type type)
        {
            var methods =
                from methodInfo in typeof (Queryable).GetMethods(BindingFlags.Public | BindingFlags.Static)
                let parameters = methodInfo.GetParameters()
                let genericParameters = methodInfo.GetGenericArguments()
                where
                    methodInfo.Name == "Count" &&
                    methodInfo.ContainsGenericParameters &&
                    parameters.Length == 1 &&
                    parameters[0].ParameterType.GetGenericTypeDefinition() == typeof (IQueryable<>)
                select methodInfo;

            MethodInfo countMethod = methods.Single().MakeGenericMethod(type);
            return countMethod;
        }
    }
}
