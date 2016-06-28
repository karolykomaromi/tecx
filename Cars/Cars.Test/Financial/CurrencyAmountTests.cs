namespace Cars.Test.Financial
{
    using System;
    using System.Globalization;
    using Cars.Financial;
    using Cars.I18n;
    using Xunit;

    public class CurrencyAmountTests
    {
        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(1.5, 2.7, 4.2)]
        public void Should_Add_If_Same_Currency(double x, double y, double expected)
        {
            Assert.Equal(expected.EUR(), x.EUR() + y.EUR());
        }

        [Theory]
        [InlineData(1, 2, -1)]
        [InlineData(2.7, 1.5, 1.2)]
        public void Should_Subtract_If_Same_Currency(double x, double y, double expected)
        {
            Assert.Equal(expected.EUR(), x.EUR() - y.EUR());
        }

        [Fact]
        public void Should_Throw_On_Add_If_Currencies_Dont_Match()
        {
            Assert.Throws<CurrencyMismatchException>(() => 1.EUR() + 2.GBP());
        }

        [Fact]
        public void Should_Throw_On_Subtract_If_Currencies_Dont_Match()
        {
            Assert.Throws<CurrencyMismatchException>(() => 1.EUR() - 2.GBP());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(1.9)]
        [InlineData(-1.9)]
        public void Should_Be_Equal(double x)
        {
            Assert.Equal(x.EUR(), x.EUR());
            Assert.True(x.EUR() == x.EUR());
        }

        [Theory]
        [InlineData(1, 1.1)]
        [InlineData(-1, 0.1)]
        [InlineData(1.9, 2)]
        [InlineData(-1.9, 0)]
        public void Should_Be_Less_Than(double x, double y)
        {
            Assert.True(x.EUR() < y.EUR());
        }

        [Theory]
        [InlineData(1.1, 1)]
        [InlineData(0.1, -1)]
        [InlineData(2, 1.9)]
        [InlineData(0, -1.9)]
        public void Should_Be_Greater_Than(double x, double y)
        {
            Assert.True(x.EUR() > y.EUR());
        }

        [Theory]
        [InlineData(1, 1.1)]
        [InlineData(-1, 0.1)]
        [InlineData(1.9, 2)]
        [InlineData(-1.9, 0)]
        [InlineData(-1.9, -1.9)]
        [InlineData(0, 0)]
        public void Should_Be_Less_Than_Or_Equal(double x, double y)
        {
            Assert.True(x.EUR() <= y.EUR());
        }

        [Theory]
        [InlineData(1.1, 1)]
        [InlineData(0.1, -1)]
        [InlineData(2, 1.9)]
        [InlineData(0, -1.9)]
        [InlineData(-1.9, -1.9)]
        [InlineData(0, 0)]
        public void Should_Be_Greater_Than_Or_Equal(double x, double y)
        {
            Assert.True(x.EUR() >= y.EUR());
        }

        [Theory]
        [InlineData(-12.345, "C3", "de-DE", "-12,345 EUR")]
        [InlineData(-12.345, "C3", "en-US", "-12.345 EUR")]
        [InlineData(-12.345, "F3", "de-DE", "-12,345")]
        [InlineData(-12.345, "F3", "en-US", "-12.345")]
        [InlineData(12.345, "C3", "de-DE", "12,345 EUR")]
        [InlineData(12.345, "C3", "en-US", "12.345 EUR")]
        [InlineData(12.345, "F3", "de-DE", "12,345")]
        [InlineData(12.345, "F3", "en-US", "12.345")]
        [InlineData(0, "C3", "de-DE", "0,000 EUR")]
        [InlineData(0, "C3", "en-US", "0.000 EUR")]
        [InlineData(0, "F3", "de-DE", "0,000")]
        [InlineData(0, "F3", "en-US", "0.000")]
        public void Should_Format_CurrencyAmount_Properly(double amount, string format, string culture, string expected)
        {
            IFormattable sut = amount.EUR();

            IFormatProvider formatProvider = CultureInfo.CreateSpecificCulture(culture);

            string actual = sut.ToString(format, formatProvider);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(-12.345, "de-DE", "-12,345 EUR")]
        public void Should_Format_Currency_Amount_Properly(double amount, string culture, string expected)
        {
            CultureInfo targetCulture = CultureInfo.CreateSpecificCulture(culture);

            using (new TemporarilyChangeCulture(targetCulture))
            {
                Assert.Equal(expected, amount.EUR().ToString());
            }
        }
    }
}
