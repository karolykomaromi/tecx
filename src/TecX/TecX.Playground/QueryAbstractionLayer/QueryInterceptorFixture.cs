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

            Expression<Func<IQueryable<Foo>, int>> count = x => x.Count();

            Expression<Func<IQueryable<Foo>, IQueryable<Foo>>> filter = x => x.Where(foo => foo.Principal.PDO_ID < 3);

            MethodCallExpression callCount = count.Body as MethodCallExpression;

            MethodCallExpression callWhere = filter.Body as MethodCallExpression;

            ParameterExpression p = Expression.Parameter(typeof(IQueryable<Foo>), "items");

            MethodCallExpression callChain = Expression.Call(callCount.Method, Expression.Call(callWhere.Method, p, callWhere.Arguments[1]));

            Expression<Func<IQueryable<Foo>, int>> all = Expression.Lambda<Func<IQueryable<Foo>, int>>(callChain, p);

            var func = all.Compile();

            Assert.Equal(2, func(items));
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
