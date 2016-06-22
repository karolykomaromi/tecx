namespace Cars.Financial
{
    using System;
    using System.Diagnostics.Contracts;

    public class Currency : IEquatable<Currency>
    {
        public static readonly Currency Empty = new Currency();

        private readonly string iso4217Code;
        private readonly short iso4217Number;
        private readonly string iso4217Name;
        private readonly string symbol;

        public Currency(string iso4217Code, short iso4217Number, string iso4217Name = "", string symbol = "")
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(iso4217Code));
            Contract.Requires(iso4217Name != null);
            Contract.Requires(symbol != null);

            this.iso4217Code = iso4217Code.ToUpperInvariant();
            this.iso4217Number = iso4217Number;
            this.iso4217Name = iso4217Name;
            this.symbol = symbol;
        }

        private Currency()
        {
            this.symbol = string.Empty;
            this.iso4217Code = string.Empty;
            this.iso4217Number = 0;
            this.iso4217Name = string.Empty;
        }

        public string Symbol
        {
            get { return this.symbol; }
        }

        public string ISO4217Code
        {
            get { return this.iso4217Code; }
        }

        public short ISO4217Number
        {
            get { return this.iso4217Number; }
        }

        public string ISO4217Name
        {
            get { return this.iso4217Name; }
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
                string.Equals(this.ISO4217Code, other.ISO4217Code, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            Currency other = obj as Currency;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.Symbol.GetHashCode() ^ this.ISO4217Code.GetHashCode();
        }
    }
}