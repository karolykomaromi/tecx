namespace Cars.Financial
{
    using System;
    using System.Diagnostics.Contracts;

    public class ExchangeRateNotFoundException : Exception
    {
        private readonly Currency sourceCurrency;
        private readonly Currency targetCurrency;

        public ExchangeRateNotFoundException(Currency sourceCurrency)
            : this(sourceCurrency, Currency.None)
        {
        }

        public ExchangeRateNotFoundException(Currency sourceCurrency, Currency targetCurrency)
        {
            Contract.Requires(sourceCurrency != null);
            Contract.Requires(targetCurrency != null);

            this.sourceCurrency = sourceCurrency;
            this.targetCurrency = targetCurrency;
        }

        public Currency SourceCurrency
        {
            get { return this.sourceCurrency; }
        }

        public Currency TargetCurrency
        {
            get { return this.targetCurrency; }
        }
    }
}