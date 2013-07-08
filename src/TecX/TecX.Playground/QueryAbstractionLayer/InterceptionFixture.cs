using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TecX.Playground.QueryAbstractionLayer
{
    using Xunit;

    public class InterceptionFixture
    {
        [Fact]
        public void Should_AlwaysBeTrue()
        {
            Assert.True(PrincipalFilter.Exclude.Filter<Foo>().Compile()(new Foo()));
        }

        [Fact]
        public void Should_BeTrueOnLeet()
        {
            Assert.True(PrincipalFilter.Include.Filter<Foo>().Compile()(new Foo { PrincipalId = 1337 }));
        }

        [Fact]
        public void Should_Wrap()
        {
            IQueryable<Foo> query = new[] { new Foo { PrincipalId = 1 }, new Foo { PrincipalId = 2 }, new Foo { PrincipalId = 1337 } }.AsQueryable();

            //IQueryable<Foo> intercepted = new QueryInterceptor<Foo>(query, new PDOperator());
            IQueryable<Foo> intercepted = new QueryInterceptor<Foo>(query, new PDOperator()).Where(f => string.IsNullOrEmpty(f.Bar));

            Assert.Equal(1, intercepted.Count());
        }
    }
}
