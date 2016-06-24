namespace Cars.Financial
{
    using System.Diagnostics.Contracts;

    public class StaticCurrencyExchange : ICurrencyExchange
    {
        private readonly ExchangeRateCollection exchangeRates;

        public StaticCurrencyExchange()
        {
            this.exchangeRates = new ExchangeRateCollection
            {
                new ExchangeRate(Currencies.CHF, Currencies.EUR, 0.9m),
                new ExchangeRate(Currencies.CHF, Currencies.GBP, 0.7m),
                new ExchangeRate(Currencies.CHF, Currencies.USD, 1.05m),
                new ExchangeRate(Currencies.EUR, Currencies.USD, 1.35m),
                new ExchangeRate(Currencies.EUR, Currencies.GBP, 0.75m),
                new ExchangeRate(Currencies.EUR, Currencies.CHF, 1.1m),
                new ExchangeRate(Currencies.GBP, Currencies.EUR, 1.3m),
                new ExchangeRate(Currencies.GBP, Currencies.USD, 1.5m),
                new ExchangeRate(Currencies.GBP, Currencies.CHF, 1.4m),
                new ExchangeRate(Currencies.USD, Currencies.EUR, 0.9m),
                new ExchangeRate(Currencies.USD, Currencies.GBP, 0.7m),
                new ExchangeRate(Currencies.USD, Currencies.CHF, 0.95m),
            };
        }

        public StaticCurrencyExchange(ExchangeRateCollection exchangeRates)
        {
            Contract.Requires(exchangeRates != null);

            this.exchangeRates = exchangeRates;
        }

        public CurrencyAmount Exchange(CurrencyAmount amount, Currency targetCurrency)
        {
            ExchangeRate exchangeRate = this.exchangeRates[amount.Currency, targetCurrency];

            return new CurrencyAmount(amount.Amount * exchangeRate.Rate, targetCurrency);
        }
    }
}