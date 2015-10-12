namespace Hydra.Cooling.Test.Alerts
{
    using Hydra.Cooling.Alerts;
    using Xunit;
    using Xunit.Extensions;

    public class PhoneExtensionTests
    {
        [Theory]
        [InlineData("-123", 123u)]
        public void Should_Parse_Valid_Strings(string s, uint expected)
        {
            PhoneExtension actual;
            Assert.True(PhoneExtension.TryParse(s, out actual));
            Assert.Equal(new PhoneExtension(expected), actual);
        }
    }
}
