namespace Cars.Financial
{
    using System;

    public class CurrencyMismatchException : Exception
    {
        public CurrencyMismatchException(CurrencyAmount x, CurrencyAmount y)
            : base(string.Format(Properties.Resources.CurrencyMismatch, x, y))
        {
        }
    }
}