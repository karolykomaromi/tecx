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
                    }
                .AsQueryable();

            IQueryable<Foo> intercepted = query.Intercept(
                new PDIteratorOperator(), 
                new ClientInfo
                    {
                        Principal = new PDPrincipal { PDO_ID = 1337 }
                    })
                .Where(f => string.IsNullOrEmpty(f.Bar));

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
                    }
                .AsQueryable();

            IQueryable<Foo> intercepted = query.Intercept(
                new PDIteratorOperator(), 
                new ClientInfo
                    {
                        Principal = new PDPrincipal { PDO_ID = 1337 }
                    });

            Assert.Equal(1, intercepted.Count());
        }

        [Fact]
        public void Show_Usage()
        {
            ISession session = new SessionImpl();

            IQueryable<Foo> query = session.Query<Foo>();

            // ...
        }
    }
}
