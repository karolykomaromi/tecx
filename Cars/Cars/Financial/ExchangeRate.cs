using System;
using System.Diagnostics.Contracts;

namespace Cars.Financial
{
    public class ExchangeRate : IEquatable<ExchangeRate>
    {
        public static readonly ExchangeRate Identity = new ExchangeRate(Currency.Empty, Currency.Empty, 1);

        private readonly Currency source;
        private readonly Currency target;
        private readonly decimal exchangeRate;

        public ExchangeRate(Currency source, Currency target, decimal exchangeRate)
        {
            Contract.Requires(source != null);
            Contract.Requires(target != null);
            Contract.Requires(exchangeRate > 0);

            this.source = source;
            this.target = target;
            this.exchangeRate = exchangeRate;
        }

        public Currency Source
        {
            get { return this.source; }
        }

        public Currency Target
        {
            get { return this.target; }
        }

        public decimal Rate
        {
            get { return this.exchangeRate; }
        }

        public static bool operator ==(ExchangeRate er1, ExchangeRate er2)
        {
            if(object.ReferenceEquals(er1, er2))
            {
                return true;
            }

            if (object.ReferenceEquals(er1, null))
            {
                return false;
            }

            if (object.ReferenceEquals(er2, null))
            {
                return false;
            }

            return er1.Equals(er2);
        }

        public static bool operator !=(ExchangeRate er1, ExchangeRate er2)
        {
            return !(er1 == er2);
        }

        public bool Equals(ExchangeRate other)
        {
            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (object.ReferenceEquals(other, null))
            {
                return false;
            }

            return this.Source == other.Source && 
                   this.Target == other.Target && 
                   this.Rate == other.Rate;
        }

        public override bool Equals(object obj)
        {
            ExchangeRate other = obj as ExchangeRate;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.Source.GetHashCode() ^ 
                   this.Target.GetHashCode() ^ 
                   this.Rate.GetHashCode();
        }
    }
}