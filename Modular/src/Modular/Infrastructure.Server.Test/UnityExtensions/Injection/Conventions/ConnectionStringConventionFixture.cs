namespace Infrastructure.Server.Test.UnityExtensions.Injection.Conventions
{
    using Infrastructure.UnityExtensions.Injection;
    using Infrastructure.UnityExtensions.Injection.Conventions;
    using Xunit;

    public class ConnectionStringConventionFixture
    {
        [Fact]
        public void Should_Match_String_On_Parameter_Names_That_Hint_To_ConnectionString()
        {
            IParameterMatchingConvention convention = new ConnectionStringConvention();

            Assert.True(convention.IsMatch(new Parameter("1"), new FakeParameterInfo("AbcConnectionString", typeof(string))));
        }
    }
}
