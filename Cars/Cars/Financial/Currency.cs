namespace Cars.Financial
{
    using System;
    using System.Diagnostics.Contracts;
    using Cars.I18n;

    public class Currency : IEquatable<Currency>
    {
        public static readonly Currency None = new Currency();

        private readonly string iso4217Code;
        private readonly short iso4217Number;
        private readonly PolyglotString name;
        private readonly string symbol;

        public Currency(PolyglotString name, string iso4217Code, short iso4217Number, string symbol = "")
        {
            Contract.Requires(name != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(iso4217Code));
            Contract.Requires(iso4217Code.Length == 3);

            this.name = name;
            this.iso4217Code = iso4217Code.ToUpperInvariant();
            this.iso4217Number = iso4217Number;
            this.symbol = symbol ?? string.Empty;
        }

        private Currency()
        {
            this.symbol = string.Empty;
            this.iso4217Code = string.Empty;
            this.iso4217Number = 0;
            this.name = PolyglotString.Empty;
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

        public PolyglotString Name
        {
            get { return this.name; }
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