using System;
using System.Diagnostics.Contracts;

namespace Cars.Financial
{
    public class Currency : IEquatable<Currency>
    {
        public static readonly Currency Empty = new Currency();

        private readonly string symbol;

        private readonly string iso;

        public Currency(string symbol, string iso)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(symbol));
            Contract.Requires(!string.IsNullOrWhiteSpace(iso));

            this.symbol = symbol;
            this.iso = iso.ToUpperInvariant();
        }

        private Currency()
        {
            this.symbol = string.Empty;
            this.iso = string.Empty;
        }

        public string Symbol
        {
            get { return this.symbol; }
        }

        public string ISO
        {
            get { return this.iso; }
        }

        public static bool operator ==(Currency c1, Currency c2)
        {
            if (object.ReferenceEquals(c1, c2))
            {
                return true;
            }

            if (object.ReferenceEquals(c1, null))
            {
                return false;
            }

            if (object.ReferenceEquals(c2, null))
            {
                return false;
            }

            return c1.Equals(c2);
        }

        public static bool operator !=(Currency c1, Currency c2)
        {
            return !(c1 == c2);
        }

        public bool Equals(Currency other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Symbol == other.Symbol &&
                string.Equals(this.ISO, other.ISO, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            Currency other = obj as Currency;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.Symbol.GetHashCode() ^ this.ISO.GetHashCode();
        }
    }
}