namespace Hydra.Cooling.Test.Alerts
{
    using Hydra.Cooling.Alerts;
    using Xunit;
    using Xunit.Extensions;

    public class DialNumberTests
    {
        [Theory]
        [InlineData("123 456 7890", 1234567890UL)]
        public void Should_Parse_Valid_Strings(string s, ulong expected)
        {
            DialNumber actual;
            Assert.True(DialNumber.TryParse(s, out actual));
            Assert.Equal(new DialNumber(expected), actual);
        }
    }
}