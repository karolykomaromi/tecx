namespace Hydra.Infrastructure.Test
{
    using Xunit;

    public class StringHelperTests
    {
        [Fact]
        public void Should_Split_Word_At_Camel_Humps()
        {
            string actual = StringHelper.SplitCamelCase("NonAuthoritativeInformation");

            string expected = "Non Authoritative Information";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Should_Leave_Acronyms_Intact()
        {
            string actual = StringHelper.SplitCamelCase("ProductID");

            string expected = "Product ID";

            Assert.Equal(expected, actual);
        }
    }
}
