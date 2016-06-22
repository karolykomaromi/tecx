namespace Cars.Financial
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public static class CurrencyAmountExtensions
    {
        public static CurrencyAmount EUR(this double amount)
        {
            return new CurrencyAmount(new decimal(amount), Currencies.EUR);
        }

        public static CurrencyAmount EUR(this int amount)
        {
            return new CurrencyAmount(new decimal(amount), Currencies.EUR);
        }

        public static CurrencyAmount USD(this double amount)
        {
            return new CurrencyAmount(new decimal(amount), Currencies.USD);
        }

        public static CurrencyAmount USD(this int amount)
        {
            return new CurrencyAmount(new decimal(amount), Currencies.USD);
        }

        public static CurrencyAmount GBP(this double amount)
        {
            return new CurrencyAmount(new decimal(amount), Currencies.GBP);
        }

        public static CurrencyAmount GBP(this int amount)
        {
            return new CurrencyAmount(new decimal(amount), Currencies.GBP);
        }

        public static CurrencyAmount CHF(this double amount)
        {
            return new CurrencyAmount(new decimal(amount), Currencies.CHF);
        }

        public static CurrencyAmount CHF(this int amount)
        {
            return new CurrencyAmount(new decimal(amount), Currencies.CHF);
        }

        public static CurrencyAmount Sum(this IEnumerable<CurrencyAmount> items)
        {
            Contract.Requires(items != null);

            CurrencyAmount sum = null;

            foreach (CurrencyAmount ca in items)
            {
                if (sum == null)
                {
                    sum = ca;
                }

                sum += ca;
            }

            return sum;
        }

        public static CurrencyAmount Sum(this IEnumerable<CurrencyAmount> items, ICurrencyExchange converter, Currency targetCurrency)
        {
            Contract.Requires(items != null);
            Contract.Requires(converter != null);
            Contract.Requires(targetCurrency != null);

            CurrencyAmount sum = null;

            foreach (CurrencyAmount ca in items)
            {
                if (sum == null)
                {
                    sum = converter.Exchange(ca, targetCurrency);
                }

                sum += converter.Exchange(ca, targetCurrency);
            }

            return sum;
        }
    }
}