namespace Cars.Test.Financial
{
    using Cars.Financial;
    using Xunit;

    public class YahooCurrencyExchangeTests
    {
        [Fact]
        public void Should_Download_And_Parse_Exchange_Rate()
        {
            ICurrencyExchange sut = new YahooCurrencyExchange();

            CurrencyAmount actual = sut.Exchange(1.EUR(), Currencies.USD);

            Assert.True(actual > 1.USD());
        }
    }
}
