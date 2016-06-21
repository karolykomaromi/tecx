using System;
using System.Diagnostics.Contracts;

namespace Cars
{
    public class Currency : IEquatable<Currency>
    {
        public static readonly Currency Default = new Currency('€', "EUR");

        public static readonly Currency Euro = new Currency('€', "EUR");

        private readonly char symbol;
        private readonly string iso;

        public Currency(char symbol, string iso)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(iso));

            this.symbol = symbol;
            this.iso = iso.ToUpperInvariant();
        }

        public char Symbol
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