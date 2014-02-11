namespace Infrastructure.Server.Test
{
    using System;
    using Xunit;

    public class TypeHelperFixture
    {
        [Fact]
        public void Should_GetSilverlightCompatibleTypeName_For_String()
        {
            string expected = "System.String, mscorlib";

            string actual = TypeHelper.GetSilverlightCompatibleTypeName(typeof(string));

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void Should_GetSilverlightCompatibleTypeName_For_Int32()
        {
            string expected = "System.Int32, mscorlib";

            string actual = TypeHelper.GetSilverlightCompatibleTypeName(typeof(int));

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void Should_GetSilverlightCompatibleTypeName_For_DateTime()
        {
            string expected = "System.DateTime, mscorlib";

            string actual = TypeHelper.GetSilverlightCompatibleTypeName(typeof(DateTime));

            Assert.Equal(expected, actual);
        }
    }
}
