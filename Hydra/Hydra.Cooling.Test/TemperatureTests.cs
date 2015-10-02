namespace Hydra.Cooling.Test
{
    using System.Globalization;
    using Hydra.Infrastructure.I18n;
    using Xunit;
    using Xunit.Extensions;

    public class TemperatureTests
    {
        [Theory]
        [InlineData(2.123456, "2.123456°C")]
        [InlineData(0, "0°C")]
        [InlineData(-2.123456, "-2.123456°C")]
        public void Should_Print_Celsius_Given_Roundtrip_Specifier_Regardless_Of_Specified_Culture(double celsius, string expected)
        {
            Celsius c = celsius.DegreesCelsius();

            Assert.Equal(expected, c.ToString(FormatStrings.Temperatures.RoundTrip, Cultures.EnglishUnitedStates));
            Assert.Equal(expected, c.ToString(FormatStrings.Temperatures.RoundTrip, Cultures.GermanGermany));
            Assert.Equal(expected, c.ToString(FormatStrings.Temperatures.RoundTrip, Cultures.FrenchNeutral));
        }

        [Theory]
        [InlineData(2.123456, "2.123456°F")]
        [InlineData(0, "0°F")]
        [InlineData(-2.123456, "-2.123456°F")]
        public void Should_Print_Fahrenheit_Given_Roundtrip_Specifier_Regardless_Of_Specified_Culture(double fahrenheit, string expected)
        {
            Fahrenheit f = fahrenheit.DegreesFahrenheit();

            Assert.Equal(expected, f.ToString(FormatStrings.Temperatures.RoundTrip, Cultures.EnglishUnitedStates));
            Assert.Equal(expected, f.ToString(FormatStrings.Temperatures.RoundTrip, Cultures.GermanGermany));
            Assert.Equal(expected, f.ToString(FormatStrings.Temperatures.RoundTrip, Cultures.FrenchNeutral));
        }

        [Theory]
        [InlineData(2.123456, "2.123456K")]
        [InlineData(0, "0K")]
        [InlineData(-2.123456, "-2.123456K")]
        public void Should_Print_Kelvin_Given_Roundtrip_Specifier_Regardless_Of_Specified_Culture(double kelvin, string expected)
        {
            Kelvin k = kelvin.Kelvin();

            Assert.Equal(expected, k.ToString(FormatStrings.Temperatures.RoundTrip, Cultures.EnglishUnitedStates));
            Assert.Equal(expected, k.ToString(FormatStrings.Temperatures.RoundTrip, Cultures.GermanGermany));
            Assert.Equal(expected, k.ToString(FormatStrings.Temperatures.RoundTrip, Cultures.FrenchNeutral));
        }

        [Theory]
        [InlineData(2.5, "2,5°C")]
        [InlineData(-2.5, "-2,5°C")]
        [InlineData(0, "0,0°C")]
        public void Should_Print_Celsius_Given_No_FormatString_And_German_Culture(double celsius, string expected)
        {
            Celsius c = celsius.DegreesCelsius();

            Assert.Equal(expected, c.ToString(null, Cultures.GermanGermany));
        }

        [Theory]
        [InlineData(2.5, "C2", "2,50°C")]
        [InlineData(-2.5, "C3", "-2,500°C")]
        [InlineData(0, "C0", "0°C")]
        public void Should_Print_Celsius_Given_Specific_FormatString_And_German_Culture(double celsius, string format, string expected)
        {
            Celsius c = celsius.DegreesCelsius();

            Assert.Equal(expected, c.ToString(format, Cultures.GermanGermany));
        }

        [Theory]
        [InlineData(2.5, "2.5°C")]
        [InlineData(-2.5, "-2.5°C")]
        [InlineData(0, "0.0°C")]
        public void Should_Print_Celsius_Given_No_FormatString_And_English_Culture(double celsius, string expected)
        {
            Celsius c = celsius.DegreesCelsius();

            Assert.Equal(expected, c.ToString(null, Cultures.EnglishUnitedStates));
        }

        [Theory]
        [InlineData(2.5, "C2", "2.50°C")]
        [InlineData(-2.5, "C3", "-2.500°C")]
        [InlineData(0, "C0", "0°C")]
        public void Should_Print_Celsius_Given_Specific_FormatString_And_English_Culture(double celsius, string format, string expected)
        {
            Celsius c = celsius.DegreesCelsius();

            Assert.Equal(expected, c.ToString(format, Cultures.EnglishUnitedStates));
        }

        [Theory]
        [InlineData(2.5, "2,5°F")]
        [InlineData(-2.5, "-2,5°F")]
        [InlineData(0, "0,0°F")]
        public void Should_Print_Fahrenheit_Given_No_FormatString_And_German_Culture(double fahrenheit, string expected)
        {
            Fahrenheit f = fahrenheit.DegreesFahrenheit();

            Assert.Equal(expected, f.ToString(null, Cultures.GermanGermany));
        }

        [Theory]
        [InlineData(2.5, "F2", "2,50°F")]
        [InlineData(-2.5, "F3", "-2,500°F")]
        [InlineData(0, "F0", "0°F")]
        public void Should_Print_Fahrenheit_Given_Specific_FormatString_And_German_Culture(double fahrenheit, string format, string expected)
        {
            Fahrenheit f = fahrenheit.DegreesFahrenheit();

            Assert.Equal(expected, f.ToString(format, Cultures.GermanGermany));
        }

        [Theory]
        [InlineData(2.5, "2.5°F")]
        [InlineData(-2.5, "-2.5°F")]
        [InlineData(0, "0.0°F")]
        public void Should_Print_Fahrenheit_Given_No_FormatString_And_English_Culture(double fahrenheit, string expected)
        {
            Fahrenheit f = fahrenheit.DegreesFahrenheit();

            Assert.Equal(expected, f.ToString(null, Cultures.EnglishUnitedStates));
        }

        [Theory]
        [InlineData(2.5, "F2", "2.50°F")]
        [InlineData(-2.5, "F3", "-2.500°F")]
        [InlineData(0, "F0", "0°F")]
        public void Should_Print_Fahrenheit_Given_Specific_FormatString_And_English_Culture(double fahrenheit, string format, string expected)
        {
            Fahrenheit f = fahrenheit.DegreesFahrenheit();

            Assert.Equal(expected, f.ToString(format, Cultures.EnglishUnitedStates));
        }

        [Theory]
        [InlineData(2.5, "2,5K")]
        [InlineData(-2.5, "-2,5K")]
        [InlineData(0, "0,0K")]
        public void Should_Print_Kelvin_Given_No_FormatString_And_German_Culture(double kelvin, string expected)
        {
            Kelvin f = kelvin.Kelvin();

            Assert.Equal(expected, f.ToString(null, Cultures.GermanGermany));
        }

        [Theory]
        [InlineData(2.5, "K2", "2,50K")]
        [InlineData(-2.5, "K3", "-2,500K")]
        [InlineData(0, "K0", "0K")]
        public void Should_Print_Kelvin_Given_Specific_FormatString_And_German_Culture(double kelvin, string format, string expected)
        {
            Kelvin f = kelvin.Kelvin();

            Assert.Equal(expected, f.ToString(format, Cultures.GermanGermany));
        }

        [Theory]
        [InlineData(2.5, "2.5K")]
        [InlineData(-2.5, "-2.5K")]
        [InlineData(0, "0.0K")]
        public void Should_Print_Kelvin_Given_No_FormatString_And_English_Culture(double kelvin, string expected)
        {
            Kelvin f = kelvin.Kelvin();

            Assert.Equal(expected, f.ToString(null, Cultures.EnglishUnitedStates));
        }

        [Theory]
        [InlineData(2.5, "K2", "2.50K")]
        [InlineData(-2.5, "K3", "-2.500K")]
        [InlineData(0, "K0", "0K")]
        public void Should_Print_Kelvin_Given_Specific_FormatString_And_English_Culture(double kelvin, string format, string expected)
        {
            Kelvin f = kelvin.Kelvin();

            Assert.Equal(expected, f.ToString(format, Cultures.EnglishUnitedStates));
        }

        [Theory]
        [InlineData("2,50°C", 2.5)]
        [InlineData("-2,5°C", -2.5)]
        [InlineData("0°C", 0)]
        public void Should_Parse_Celsius_Given_German_Culture(string s, double temperature)
        {
            Celsius expected = temperature.DegreesCelsius();

            Temperature actual;
            Assert.True(Temperature.TryParse(s, NumberStyles.Number, Cultures.GermanGermany, out actual));
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("2.50°C", 2.5)]
        [InlineData("-2.5°C", -2.5)]
        [InlineData("0°C", 0)]
        public void Should_Parse_Celsius_Given_English_Culture(string s, double temperature)
        {
            Celsius expected = temperature.DegreesCelsius();

            Temperature actual;
            Assert.True(Temperature.TryParse(s, NumberStyles.Number, Cultures.EnglishUnitedStates, out actual));
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("2,50°F", 2.5)]
        [InlineData("-2,5°F", -2.5)]
        [InlineData("0°F", 0)]
        public void Should_Parse_Fahrenheit_Given_German_Culture(string s, double temperature)
        {
            Fahrenheit expected = temperature.DegreesFahrenheit();

            Temperature actual;
            Assert.True(Temperature.TryParse(s, NumberStyles.Number, Cultures.GermanGermany, out actual));
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("2.50°F", 2.5)]
        [InlineData("-2.5°F", -2.5)]
        [InlineData("0°F", 0)]
        public void Should_Parse_Fahrenheit_Given_English_Culture(string s, double temperature)
        {
            Fahrenheit expected = temperature.DegreesFahrenheit();

            Temperature actual;
            Assert.True(Temperature.TryParse(s, NumberStyles.Number, Cultures.EnglishUnitedStates, out actual));
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("2,50K", 2.5)]
        [InlineData("-2,5K", -2.5)]
        [InlineData("0K", 0)]
        public void Should_Parse_Kelvin_Given_German_Culture(string s, double temperature)
        {
            Kelvin expected = temperature.Kelvin();

            Temperature actual;
            Assert.True(Temperature.TryParse(s, NumberStyles.Number, Cultures.GermanGermany, out actual));
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("2.50K", 2.5)]
        [InlineData("-2.5K", -2.5)]
        [InlineData("0K", 0)]
        public void Should_Parse_Kelvin_Given_English_Culture(string s, double temperature)
        {
            Kelvin expected = temperature.Kelvin();

            Temperature actual;
            Assert.True(Temperature.TryParse(s, NumberStyles.Number, Cultures.EnglishUnitedStates, out actual));
            Assert.Equal(expected, actual);
        }
    }
}