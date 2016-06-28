namespace Cars.Financial
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public class ExchangeRateCollection : IEnumerable<ExchangeRate>
    {
        private readonly IDictionary<Tuple<Currency, Currency>, ExchangeRate> exchangeRates;

        public ExchangeRateCollection()
        {
            this.exchangeRates = new Dictionary<Tuple<Currency, Currency>, ExchangeRate>();
        }

        public ExchangeRate this[Currency source, Currency target]
        {
            get
            {
                Contract.Requires(source != null);
                Contract.Requires(target != null);

                if (source == target)
                {
                    return ExchangeRate.Identity;
                }

                var key = Key(source, target);

                ExchangeRate rate;
                if (!this.exchangeRates.TryGetValue(key, out rate))
                {
                    throw new ExchangeRateNotFoundException(source, target);
                }

                return rate;
            }
        }

        public bool TryGetExchangeRate(Currency source, Currency target, out ExchangeRate exchangeRate)
        {
            var key = Key(source, target);
            if (this.exchangeRates.TryGetValue(key, out exchangeRate))
            {
                return true;
            }

            return false;
        }

        public void Add(ExchangeRate exchangeRate)
        {
            var key = Key(exchangeRate.Source, exchangeRate.Target);

            this.exchangeRates[key] = exchangeRate;
        }

        public IEnumerator<ExchangeRate> GetEnumerator()
        {
            return this.exchangeRates.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private static Tuple<Currency, Currency> Key(Currency source, Currency target)
        {
            return new Tuple<Currency, Currency>(source, target);
        }
    }
}