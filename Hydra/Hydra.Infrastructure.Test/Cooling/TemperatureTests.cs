namespace Hydra.Infrastructure.Test.Cooling
{
    using Hydra.Infrastructure.Cooling;
    using Hydra.Infrastructure.I18n;
    using Xunit;
    using Xunit.Extensions;

    public class TemperatureTests
    {
        [Theory]
        [InlineData(2.5, "2,5°C")]
        [InlineData(-2.5, "-2,5°C")]
        [InlineData(0, "0,0°C")]
        public void Should_Correctly_Print_Celsius_Given_No_FormatString_And_German_Culture(double celsius, string expected)
        {
            Celsius c = celsius.DegreesCelsius();

            Assert.Equal(expected, c.ToString(null, Cultures.GermanGermany));
        }

        [Theory]
        [InlineData(2.5, "C2", "2,50°C")]
        [InlineData(-2.5, "C3", "-2,500°C")]
        [InlineData(0, "C0", "0°C")]
        public void Should_Correctly_Print_Celsius_Given_Specific_FormatString_And_German_Culture(double celsius, string format, string expected)
        {
            Celsius c = celsius.DegreesCelsius();

            Assert.Equal(expected, c.ToString(format, Cultures.GermanGermany));
        }

        [Theory]
        [InlineData(2.5, "2.5°C")]
        [InlineData(-2.5, "-2.5°C")]
        [InlineData(0, "0.0°C")]
        public void Should_Correctly_Print_Celsius_Given_No_FormatString_And_English_Culture(double celsius, string expected)
        {
            Celsius c = celsius.DegreesCelsius();

            Assert.Equal(expected, c.ToString(null, Cultures.EnglishUnitedStates));
        }

        [Theory]
        [InlineData(2.5, "C2", "2.50°C")]
        [InlineData(-2.5, "C3", "-2.500°C")]
        [InlineData(0, "C0", "0°C")]
        public void Should_Correctly_Print_Celsius_Given_Specific_FormatString_And_English_Culture(double celsius, string format, string expected)
        {
            Celsius c = celsius.DegreesCelsius();

            Assert.Equal(expected, c.ToString(format, Cultures.EnglishUnitedStates));
        }

        [Theory]
        [InlineData(2.5, "2,5°F")]
        [InlineData(-2.5, "-2,5°F")]
        [InlineData(0, "0,0°F")]
        public void Should_Correctly_Print_Fahrenheit_Given_No_FormatString_And_German_Culture(double fahrenheit, string expected)
        {
            Fahrenheit f = fahrenheit.DegreesFahrenheit();

            Assert.Equal(expected, f.ToString(null, Cultures.GermanGermany));
        }

        [Theory]
        [InlineData(2.5, "F2", "2,50°F")]
        [InlineData(-2.5, "F3", "-2,500°F")]
        [InlineData(0, "F0", "0°F")]
        public void Should_Correctly_Print_Fahrenheit_Given_Specific_FormatString_And_German_Culture(double fahrenheit, string format, string expected)
        {
            Fahrenheit f = fahrenheit.DegreesFahrenheit();

            Assert.Equal(expected, f.ToString(format, Cultures.GermanGermany));
        }

        [Theory]
        [InlineData(2.5, "2.5°F")]
        [InlineData(-2.5, "-2.5°F")]
        [InlineData(0, "0.0°F")]
        public void Should_Correctly_Print_Fahrenheit_Given_No_FormatString_And_English_Culture(double fahrenheit, string expected)
        {
            Fahrenheit f = fahrenheit.DegreesFahrenheit();

            Assert.Equal(expected, f.ToString(null, Cultures.EnglishUnitedStates));
        }

        [Theory]
        [InlineData(2.5, "F2", "2.50°F")]
        [InlineData(-2.5, "F3", "-2.500°F")]
        [InlineData(0, "F0", "0°F")]
        public void Should_Correctly_Print_Fahrenheit_Given_Specific_FormatString_And_English_Culture(double fahrenheit, string format, string expected)
        {
            Fahrenheit f = fahrenheit.DegreesFahrenheit();

            Assert.Equal(expected, f.ToString(format, Cultures.EnglishUnitedStates));
        }

        [Theory]
        [InlineData(2.5, "2,5K")]
        [InlineData(-2.5, "-2,5K")]
        [InlineData(0, "0,0K")]
        public void Should_Correctly_Print_Kelvin_Given_No_FormatString_And_German_Culture(double kelvin, string expected)
        {
            Kelvin f = kelvin.Kelvin();

            Assert.Equal(expected, f.ToString(null, Cultures.GermanGermany));
        }

        [Theory]
        [InlineData(2.5, "K2", "2,50K")]
        [InlineData(-2.5, "K3", "-2,500K")]
        [InlineData(0, "K0", "0K")]
        public void Should_Correctly_Print_Kelvin_Given_Specific_FormatString_And_German_Culture(double kelvin, string format, string expected)
        {
            Kelvin f = kelvin.Kelvin();

            Assert.Equal(expected, f.ToString(format, Cultures.GermanGermany));
        }

        [Theory]
        [InlineData(2.5, "2.5K")]
        [InlineData(-2.5, "-2.5K")]
        [InlineData(0, "0.0K")]
        public void Should_Correctly_Print_Kelvin_Given_No_FormatString_And_English_Culture(double kelvin, string expected)
        {
            Kelvin f = kelvin.Kelvin();

            Assert.Equal(expected, f.ToString(null, Cultures.EnglishUnitedStates));
        }

        [Theory]
        [InlineData(2.5, "K2", "2.50K")]
        [InlineData(-2.5, "K3", "-2.500K")]
        [InlineData(0, "K0", "0K")]
        public void Should_Correctly_Print_Kelvin_Given_Specific_FormatString_And_English_Culture(double kelvin, string format, string expected)
        {
            Kelvin f = kelvin.Kelvin();

            Assert.Equal(expected, f.ToString(format, Cultures.EnglishUnitedStates));
        }
    }
}