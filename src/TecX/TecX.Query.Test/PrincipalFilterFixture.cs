namespace TecX.Query.Test
{
    using TecX.Query.Filters;
    using TecX.Query.PD;

    using Xunit;

    public class PrincipalFilterFixture
    {
        [Fact]
        public void Disabled_Should_AlwaysBeTrue()
        {
            IClientInfo clientInfo = new ClientInfo();

            Assert.True(PrincipalFilter.Disabled.Filter<Foo>(clientInfo).Compile()(new Foo()));
            Assert.True(PrincipalFilter.Disabled.Filter<Foo>(clientInfo).Compile()(new Foo { Principal = new PDPrincipal { PDO_ID = 1337 } }));
        }

        [Fact]
        public void Enabled_Should_BeTrueOnMatchingPrincipalId()
        {
            IClientInfo clientInfo = new ClientInfo { Principal = new PDPrincipal { PDO_ID = 1337 } };

            Assert.True(PrincipalFilter.Enabled.Filter<Foo>(clientInfo).Compile()(new Foo { Principal = new PDPrincipal { PDO_ID = 1337 } }));
        }

        [Fact]
        public void Enabled_Should_BeFalse_OnMismatchingPrincipalId()
        {
            IClientInfo clientInfo = new ClientInfo { Principal = new PDPrincipal { PDO_ID = 1337 } };

            Assert.False(PrincipalFilter.Enabled.Filter<Foo>(clientInfo).Compile()(new Foo { Principal = new PDPrincipal { PDO_ID = 1 } }));
        }
    }
}