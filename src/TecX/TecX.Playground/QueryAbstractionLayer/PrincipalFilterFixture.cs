using TecX.Playground.QueryAbstractionLayer.Filters;
using TecX.Playground.QueryAbstractionLayer.PD;

using Xunit;

namespace TecX.Playground.QueryAbstractionLayer
{
    public class PrincipalFilterFixture
    {
        [Fact]
        public void Disabled_Should_AlwaysBeTrue()
        {
            Assert.True(PrincipalFilter.Disabled.Filter<Foo>().Compile()(new Foo()));
            Assert.True(PrincipalFilter.Disabled.Filter<Foo>().Compile()(new Foo { PrincipalId = 1337 }));
        }

        [Fact]
        public void Enabled_Should_BeTrueOnMatchingPrincipalId()
        {
            Assert.True(PrincipalFilter.Enabled.Filter<Foo>().Compile()(new Foo { PrincipalId = 1337 }));
        }

        [Fact]
        public void Enabled_Should_BeFalse_OnMismatchingPrincipalId()
        {
            Assert.False(PrincipalFilter.Enabled.Filter<Foo>().Compile()(new Foo { PrincipalId = 1 }));
        }
    }
}