namespace Cars.Financial
{
    using System;
    using System.Globalization;
    using System.Net;
    using Cars.I18n;

    public class YahooCurrencyExchange : ICurrencyExchange
    {
        private const string YahooUri = @"http://download.finance.yahoo.com/d/quotes.csv?s={0}{1}=X&f=sl1d1t1ba&e=.csv";
        private readonly ExchangeRateCollection exchangeRates;

        public YahooCurrencyExchange()
        {
            this.exchangeRates = new ExchangeRateCollection();
        }

        public CurrencyAmount Exchange(CurrencyAmount amount, Currency targetCurrency)
        {
            Currency sourceCurrency = amount.Currency;

            if (sourceCurrency == targetCurrency)
            {
                return amount;
            }

            ExchangeRate exchangeRate;
            if (!this.exchangeRates.TryGetExchangeRate(sourceCurrency, targetCurrency, out exchangeRate))
            {
                WebClient client = new WebClient();

                Uri uri = new Uri(string.Format(YahooUri, sourceCurrency.ISO4217Code, targetCurrency.ISO4217Code), UriKind.Absolute);

                string csv;
                try
                {
                    csv = client.DownloadString(uri);
                }
                catch (Exception exception)
                {
                    throw new ExchangeRateNotFoundException(sourceCurrency, targetCurrency, exception);
                }

                if (string.IsNullOrEmpty(csv))
                {
                    throw new ExchangeRateNotFoundException(sourceCurrency, targetCurrency);
                }

                decimal er;
                if (!TryParseCsv(csv, out er))
                {
                    throw new ExchangeRateNotFoundException(sourceCurrency, targetCurrency);
                }

                exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, er);
                this.exchangeRates.Add(exchangeRate);
            }

            return amount.Exchange(exchangeRate);
        }

        private static bool TryParseCsv(string csv, out decimal exchangeRate)
        {
            string[] s = csv.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (s.Length < 2)
            {
                exchangeRate = decimal.MinValue;
                return false;
            }

            if (!decimal.TryParse(s[1], NumberStyles.Float, Cultures.EnglishUnitedStates, out exchangeRate))
            {
                return false;
            }

            return true;
        }
    }
}