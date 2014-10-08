namespace Infrastructure.Server.Test.UnityExtensions.Injection.Conventions
{
    using Infrastructure.UnityExtensions.Injection;
    using Infrastructure.UnityExtensions.Injection.Conventions;
    using Xunit;

    public class SpecifiedNameConventionFixture
    {
        [Fact]
        public void Should_Match_On_Specified_Parameter_Name()
        {
            IParameterMatchingConvention convention = new SpecifiedNameConvention();

            Assert.True(convention.IsMatch(new Parameter("abc", "1"), new FakeParameterInfo("abc", typeof(string))));
        }

        [Fact]
        public void Should_Not_Match_On_Specified_Parameter_Name_But_Non_Assignable_Type()
        {
            IParameterMatchingConvention convention = new SpecifiedNameConvention();

            Assert.False(convention.IsMatch(new Parameter("abc", 1), new FakeParameterInfo("abc", typeof(string))));
        }
    }
}
