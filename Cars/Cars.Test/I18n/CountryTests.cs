namespace Cars.Test.I18n
{
    using Cars.I18n;
    using Xunit;

    public class CountryTests
    {
        [Fact]
        public void Should_Get_Currencies_For_Country()
        {
            var expected = Country2Currencies.BTN;

            var actual = Countries.BTN.Currencies;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Should_Get_Cultures_For_Country()
        {
            var actual = Countries.CHE.Cultures;

            Assert.Equal(6, actual.Count);
        }

        [Fact]
        public void Should_Get_TimeZone_For_Country()
        {
            var actual = Countries.AUS.TimeZones;

            // "Central Pacific Standard Time"
            // "Tasmania Standard Time"
            // "AUS Eastern Standard Time"
            // "Cen. Australia Standard Time"
            // "E. Australia Standard Time"
            // "AUS Central Standard Time"
            // "W. Australia Standard Time"
            Assert.Equal(7, actual.Count);
        }
    }
}
