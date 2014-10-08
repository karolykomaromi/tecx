using Infrastructure.Server.Test.TestObjects;

namespace Infrastructure.Server.Test.UnityExtensions.Injection.Conventions
{
    using Infrastructure.UnityExtensions.Injection;
    using Infrastructure.UnityExtensions.Injection.Conventions;
    using Xunit;

    public class StringAsMappingNameConventionFixture
    {
        [Fact]
        public void Should_Match_String_As_Mapping_Name_For_Non_Primitive_Parameter_Types()
        {
            IParameterMatchingConvention convention = new StringAsMappingNameConvention();

            Assert.True(convention.IsMatch(new Parameter("Abc", "1"), new FakeParameterInfo("Abc", typeof(IFoo))));
        }

        [Fact]
        public void Should_Not_Match_String_As_Mapping_Name_For_Primitive_Parameter_Types()
        {
            var convention = new StringAsMappingNameConvention();

            Assert.False(convention.IsMatch(new Parameter("Abc", "1"), new FakeParameterInfo("Abc", typeof(int))));
        }
    }
}
