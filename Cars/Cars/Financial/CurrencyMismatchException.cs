namespace Cars.Financial
{
    using System;

    public class CurrencyMismatchException : Exception
    {
        public CurrencyMismatchException(CurrencyAmount x, CurrencyAmount y)
            : base(string.Format(Properties.Resources.CurrencyMismatch, x, y))
        {
        }

        public CurrencyMismatchException(Currency x, Currency y)
            : base(string.Format(Properties.Resources.CurrencyMismatch, x, y))
        {
        }
    }
}