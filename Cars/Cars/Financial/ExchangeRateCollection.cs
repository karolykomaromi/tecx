using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Cars.Financial
{
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
                    return ExchangeRate.Empty;
                }

                var key = new Tuple<Currency, Currency>(source, target);

                ExchangeRate rate;
                if (!this.exchangeRates.TryGetValue(key, out rate))
                {
                    throw new ExchangeRateNotFoundException(source, target);
                }

                return rate;
            }
        }

        public void Add(ExchangeRate exchangeRate)
        {
            var key = new Tuple<Currency, Currency>(exchangeRate.Source, exchangeRate.Target);

            this.exchangeRates[key] = exchangeRate;
        }

        public IEnumerator<ExchangeRate> GetEnumerator()
        {
            return this.exchangeRates.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}