namespace Cars.I18n
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using Cars.Financial;
    using NodaTime.TimeZones;

    public class Country : IEquatable<Country>
    {
        public static readonly Country None = new Country();

        private readonly PolyglotString name;
        private readonly string alpha2Code;
        private readonly string alpha3Code;
        private readonly short number;
        private readonly Lazy<IReadOnlyCollection<Currency>> currencies;
        private readonly Lazy<IReadOnlyCollection<CultureInfo>> cultures;
        private readonly Lazy<IReadOnlyCollection<TimeZoneInfo>> timezones;

        public Country(PolyglotString name, string alpha2Code, string alpha3Code, short number)
        {
            Contract.Requires(name != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(alpha2Code));
            Contract.Requires(alpha2Code.Length == 2);
            Contract.Requires(!string.IsNullOrWhiteSpace(alpha3Code));
            Contract.Requires(alpha3Code.Length == 3);
            Contract.Requires(number > 0);

            this.name = name;
            this.alpha2Code = alpha2Code.ToUpperInvariant();
            this.alpha3Code = alpha3Code.ToUpperInvariant();
            this.number = number;

            this.currencies = new Lazy<IReadOnlyCollection<Currency>>(this.GetCurrencies);
            this.cultures = new Lazy<IReadOnlyCollection<CultureInfo>>(this.GetCultures);
            this.timezones = new Lazy<IReadOnlyCollection<TimeZoneInfo>>(this.GetTimeZones);
        }

        private Country()
        {
            this.name = PolyglotString.Empty;
            this.alpha2Code = string.Empty;
            this.alpha3Code = string.Empty;
            this.number = -1;

            this.currencies = new Lazy<IReadOnlyCollection<Currency>>(this.GetCurrencies);
            this.cultures = new Lazy<IReadOnlyCollection<CultureInfo>>(this.GetCultures);
            this.timezones = new Lazy<IReadOnlyCollection<TimeZoneInfo>>(this.GetTimeZones);
        }

        public PolyglotString Name
        {
            get
            {
                Contract.Ensures(Contract.Result<PolyglotString>() != null);

                return this.name;
            }
        }

        public string Alpha2Code
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);

                return this.alpha2Code;
            }
        }

        public string Alpha3Code
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);

                return this.alpha3Code;
            }
        }

        public short Number
        {
            get { return this.number; }
        }

        public IReadOnlyCollection<Currency> Currencies
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyCollection<Currency>>() != null);

                return this.currencies.Value;
            }
        }

        public IReadOnlyCollection<CultureInfo> Cultures
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyCollection<CultureInfo>>() != null);

                return this.cultures.Value;
            }
        }

        public IReadOnlyCollection<TimeZoneInfo> TimeZones
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyCollection<TimeZoneInfo>>() != null);

                return this.timezones.Value;
            }
        }

        public static bool operator ==(Country c1, Country c2)
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

        public static bool operator !=(Country c1, Country c2)
        {
            return !(c1 == c2);
        }

        public bool Equals(Country other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }

            return this.Number == other.Number;
        }

        public override bool Equals(object obj)
        {
            Country other = obj as Country;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.Number;
        }

        public override string ToString()
        {
            return this.Name.ToString();
        }

        private IReadOnlyCollection<Currency> GetCurrencies()
        {
            if (this.Number == Country.None.Number)
            {
                return new[] { Currency.None, };
            }

            FieldInfo field = typeof(Country2Currencies).GetField(this.Alpha3Code, BindingFlags.Static | BindingFlags.Public);

            if (field != null)
            {
                IReadOnlyCollection<Currency> cs = field.GetValue(null) as IReadOnlyCollection<Currency>;

                if (cs != null)
                {
                    return cs;
                }
            }

            return new[] { Currency.None, };
        }

        private IReadOnlyCollection<CultureInfo> GetCultures()
        {
            if (this.Number == None.Number)
            {
                return new CultureInfo[0];
            }

            var cs = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(c => c.Name.EndsWith(this.Alpha2Code, StringComparison.OrdinalIgnoreCase))
                .ToArray();

            return cs;
        }

        private IReadOnlyCollection<TimeZoneInfo> GetTimeZones()
        {
            if (this.Number == None.Number)
            {
                return new TimeZoneInfo[0];
            }

            // http://stackoverflow.com/questions/19695439/get-the-default-timezone-for-a-country-via-cultureinfo
            var windowsTimeZones = TzdbDateTimeZoneSource.Default.ZoneLocations
                .Where(x => string.Equals(x.CountryCode, this.Alpha2Code, StringComparison.Ordinal))
                .Select(tz => TzdbDateTimeZoneSource.Default.WindowsMapping.MapZones.FirstOrDefault(x => x.TzdbIds.Contains(tz.ZoneId)))
                .Where(x => x != null)
                .Select(x => TimeZoneInfo.FindSystemTimeZoneById(x.WindowsId))
                .Distinct()
                .ToArray();

            return windowsTimeZones;
        }
    }
}
