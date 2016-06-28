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
            : base(string.Format(Properties.Resources.ExchangeRateNotFound, sourceCurrency.ISO4217Code, targetCurrency.ISO4217Code))
        {
            Contract.Requires(sourceCurrency != null);
            Contract.Requires(targetCurrency != null);

            this.sourceCurrency = sourceCurrency;
            this.targetCurrency = targetCurrency;
        }

        public ExchangeRateNotFoundException(Currency sourceCurrency, Currency targetCurrency, Exception innerException)
            : base(string.Format(Properties.Resources.ExchangeRateNotFound, sourceCurrency.ISO4217Code, targetCurrency.ISO4217Code), innerException)
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