using System;
using System.Diagnostics.Contracts;

namespace Cars.Financial
{
    public class ExchangeRateNotFoundException : Exception
    {
        private readonly Currency source;
        private readonly Currency target;

        public ExchangeRateNotFoundException(Currency source)
            : this(source, Currency.Empty)
        {
        }

        public ExchangeRateNotFoundException(Currency source, Currency target)
        {
            Contract.Requires(source != null);
            Contract.Requires(target != null);

            this.source = source;
            this.target = target;
        }

        public Currency Source
        {
            get { return this.source; }
        }

        public Currency Target
        {
            get { return this.target; }
        }
    }
}