using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Cars
{
    public static class CurrencyAmountExtensions
    {
        public static CurrencyAmount Euro(this double amount)
        {
            return new CurrencyAmount(new decimal(amount), Currency.Euro);
        }

        public static CurrencyAmount Euro(this int amount)
        {
            return new CurrencyAmount(new decimal(amount), Currency.Euro);
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
    }
}