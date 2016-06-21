using System.Diagnostics.Contracts;

namespace Cars.Financial
{
    public class StaticCurrencyConverter : ICurrencyConverter
    {
        private readonly ExchangeRateCollection exchangeRates;

        public StaticCurrencyConverter()
        {
            this.exchangeRates = new ExchangeRateCollection
            {
                new ExchangeRate(Currencies.EUR, Currencies.USD, 1.35m),
                new ExchangeRate(Currencies.EUR, Currencies.GBP, 0.75m),
                new ExchangeRate(Currencies.USD, Currencies.EUR, 0.9m),
                new ExchangeRate(Currencies.USD, Currencies.GBP, 0.7m),
                new ExchangeRate(Currencies.GBP, Currencies.EUR, 1.3m),
                new ExchangeRate(Currencies.GBP, Currencies.USD, 1.5m),
            };
        }

        public StaticCurrencyConverter(ExchangeRateCollection exchangeRates)
        {
            Contract.Requires(exchangeRates != null);

            this.exchangeRates = exchangeRates;
        }

        public CurrencyAmount Convert(CurrencyAmount amount, Currency targetCurrency)
        {
            ExchangeRate exchangeRate = this.exchangeRates[amount.Currency, targetCurrency];

            return new CurrencyAmount(amount.Amount * exchangeRate.Rate, targetCurrency);
        }
    }
}