namespace Cars.Test.Financial
{
    using Cars.Financial;
    using Xunit;

    public class CurrencyTests
    {
        [Fact]
        public void Should_Get_Countries_For_Currency()
        {
            var expected = Currency2Countries.EUR;

            var actual = Currencies.EUR.Countries;

            Assert.Equal(expected, actual);
        }
    }
}
