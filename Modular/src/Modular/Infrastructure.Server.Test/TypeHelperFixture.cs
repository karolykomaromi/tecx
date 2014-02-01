namespace Infrastructure.Server.Test
{
    using Xunit;

    public class TypeHelperFixture
    {
        [Fact]
        public void Should_Get_TypeName_And_Simple_AssemblyName()
        {
            string expected = "System.String, mscorlib";

            string actual = TypeHelper.GetSilverlightCompatibleTypeName(typeof(string));

            Assert.Equal(expected, actual);
        }
    }
}
