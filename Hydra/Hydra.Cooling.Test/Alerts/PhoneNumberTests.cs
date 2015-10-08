namespace Hydra.Cooling.Test.Alerts
{
    using Hydra.Cooling.Alerts;
    using Xunit;
    using Xunit.Extensions;

    public class PhoneNumberTests
    {
        [Fact]
        public void Should_Print_Empty_Phone_Number_As_Empty_String()
        {
            string actual = PhoneNumber.Empty.ToString();

            Assert.Equal(string.Empty, actual);
        }

        [Theory]
        [InlineData(FormatStrings.PhoneNumbers.General, (ushort)49, 721u, 47049050UL, 0u, "+49 (721) 47049050")]
        [InlineData(FormatStrings.PhoneNumbers.General, (ushort)41, 123u, 4567890UL, 123u, "+41 (123) 4567890 - 123")]
        [InlineData(FormatStrings.PhoneNumbers.Number, (ushort)49, 721u, 47049050UL, 0u, "4972147049050")]
        [InlineData(FormatStrings.PhoneNumbers.Number, (ushort)41, 123u, 4567890UL, 123u, "411234567890123")]
        public void Should_Print_Phone_Number_Correctly(
            string format,
            ushort countryCode,
            uint areaCode,
            ulong dialNumber,
            uint phoneExtension,
            string expected)
        {
            string actual = new PhoneNumber(
                new CountryCode(countryCode), 
                new AreaCode(areaCode), 
                new DialNumber(dialNumber), 
                new PhoneExtension(phoneExtension)).ToString(format);

            Assert.Equal(expected, actual);
        }
    }
}
