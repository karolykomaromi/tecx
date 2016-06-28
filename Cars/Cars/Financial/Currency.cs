namespace Cars.Financial
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using Cars.I18n;

    public class Currency : IEquatable<Currency>
    {
        public static readonly Currency None = new Currency();

        private readonly string iso4217Code;
        private readonly short iso4217Number;
        private readonly PolyglotString name;
        private readonly string symbol;
        private readonly Lazy<IReadOnlyCollection<Country>> countries;

        public Currency(PolyglotString name, string iso4217Code, short iso4217Number, string symbol = "")
        {
            Contract.Requires(name != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(iso4217Code));
            Contract.Requires(iso4217Code.Length == 3);

            this.name = name;
            this.iso4217Code = iso4217Code.ToUpperInvariant();
            this.iso4217Number = iso4217Number;
            this.symbol = symbol ?? string.Empty;
            this.countries = new Lazy<IReadOnlyCollection<Country>>(this.GetCountries);
        }

        private Currency()
        {
            this.symbol = string.Empty;
            this.iso4217Code = string.Empty;
            this.iso4217Number = 0;
            this.name = PolyglotString.Empty;
            this.countries = new Lazy<IReadOnlyCollection<Country>>(this.GetCountries);
        }

        public string Symbol
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);

                return this.symbol;
            }
        }

        public string ISO4217Code
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);

                return this.iso4217Code;
            }
        }

        public short ISO4217Number
        {
            get { return this.iso4217Number; }
        }

        public PolyglotString Name
        {
            get
            {
                Contract.Ensures(Contract.Result<PolyglotString>() != null);

                return this.name;
            }
        }

        public IReadOnlyCollection<Country> Countries
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyCollection<Country>>() != null);

                return this.countries.Value;
            }
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

            return this.ISO4217Number == other.iso4217Number;
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

        private IReadOnlyCollection<Country> GetCountries()
        {
            if (this.ISO4217Number == Currency.None.ISO4217Number)
            {
                return new Country[0];
            }

            FieldInfo field = typeof(Currency2Countries).GetField(this.ISO4217Code, BindingFlags.Static | BindingFlags.Public);

            if (field != null)
            {
                IReadOnlyCollection<Country> cs = field.GetValue(null) as IReadOnlyCollection<Country>;

                if (cs != null)
                {
                    return cs;
                }
            }

            return new Country[0];
        }
    }
}