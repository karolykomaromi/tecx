namespace Hydra.Cooling.Test.Alerts
{
    using Hydra.Cooling.Alerts;
    using Hydra.Infrastructure.I18n;
    using Xunit;
    using Xunit.Extensions;

    public class CountryCodeTests
    {
        [Theory]
        [InlineData("+49", (ushort)49)]
        [InlineData("49", (ushort)49)]
        public void Should_Parse_Valid_Strings(string s, ushort expected)
        {
            CountryCode actual;
            Assert.True(CountryCode.TryParse(s, out actual));
            Assert.Equal(new CountryCode(expected), actual);
        }

        [Fact]
        public void Should_Print_Empty_CountryCode_As_Empty_String()
        {
            string actual = CountryCode.Empty.ToString();

            Assert.Equal(string.Empty, actual);
        }

        [Fact]
        public void Should_Return_German_Country_Code()
        {
            CountryCode actual;

            Assert.True(CountryCode.TryGetCountryCode(Cultures.GermanGermany, out actual));
            Assert.Equal(CountryCodes.Germany, actual);

            Assert.True(CountryCode.TryGetCountryCode(Cultures.GermanNeutral, out actual));
            Assert.Equal(CountryCodes.Germany, actual);
        }

        [Fact]
        public void Should_Return_Austrian_Country_Code()
        {
            CountryCode actual;

            Assert.True(CountryCode.TryGetCountryCode(Cultures.GermanAustria, out actual));
            Assert.Equal(CountryCodes.Austria, actual);
        }

        [Fact]
        public void Should_Return_Swisse_County_Code()
        {
            CountryCode actual;

            Assert.True(CountryCode.TryGetCountryCode(Cultures.FrenchSwitzerland, out actual));
            Assert.Equal(CountryCodes.Switzerland, actual);

            Assert.True(CountryCode.TryGetCountryCode(Cultures.GermanSwitzerland, out actual));
            Assert.Equal(CountryCodes.Switzerland, actual);

            Assert.True(CountryCode.TryGetCountryCode(Cultures.ItalianSwitzerland, out actual));
            Assert.Equal(CountryCodes.Switzerland, actual);

            Assert.True(CountryCode.TryGetCountryCode(Cultures.RomanshSwitzerland, out actual));
            Assert.Equal(CountryCodes.Switzerland, actual);
        }

        [Fact]
        public void Should_Return_French_Country_Code()
        {
            CountryCode actual;

            Assert.True(CountryCode.TryGetCountryCode(Cultures.FrenchFrance, out actual));
            Assert.Equal(CountryCodes.France, actual);
        }

        [Fact]
        public void Should_Return_British_Country_Code()
        {
            CountryCode actual;

            Assert.True(CountryCode.TryGetCountryCode(Cultures.EnglishUnitedKingdom, out actual));
            Assert.Equal(CountryCodes.UnitedKingdom, actual);

            Assert.True(CountryCode.TryGetCountryCode(Cultures.WelshUnitedKingdom, out actual));
            Assert.Equal(CountryCodes.UnitedKingdom, actual);

            Assert.True(CountryCode.TryGetCountryCode(Cultures.ScottishGaelicUnitedKingdom, out actual));
            Assert.Equal(CountryCodes.UnitedKingdom, actual);
        }
    }
}