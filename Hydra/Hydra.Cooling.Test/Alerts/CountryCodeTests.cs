namespace Hydra.Cooling.Test.Alerts
{
    using Hydra.Cooling.Alerts;
    using Hydra.Infrastructure.I18n;
    using Xunit;

    public class CountryCodeTests
    {
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

            //var cultures =
            //    CultureInfo.GetCultures(CultureTypes.AllCultures | CultureTypes.NeutralCultures)
            //        .Where(culture => culture.Name.EndsWith("CH", StringComparison.Ordinal)).ToArray();
        }
    }
}