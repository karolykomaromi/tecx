namespace Hydra.Cooling.Alerts
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Hydra.Infrastructure.I18n;

    public class CountryCode : IEquatable<CountryCode>, IComparable<CountryCode>, IFormattable
    {
        public static readonly CountryCode Empty = new CountryCode();

        private readonly ushort countryCode;

        public CountryCode(ushort countryCode)
        {
            this.countryCode = countryCode;
        }

        private CountryCode()
        {
        }

        public static bool operator ==(CountryCode first, CountryCode second)
        {
            if (object.ReferenceEquals(first, second))
            {
                return true;
            }

            if (object.ReferenceEquals(first, null))
            {
                return false;
            }

            if (object.ReferenceEquals(second, null))
            {
                return false;
            }

            return first.Equals(second);
        }

        public static bool operator !=(CountryCode first, CountryCode second)
        {
            return !(first == second);
        }

        public static bool TryGetCountryCode(CultureInfo culture, out CountryCode countryCode)
        {
            if (culture == null)
            {
                countryCode = CountryCode.Empty;
                return false;
            }

            if (culture.Equals(Cultures.GermanGermany) || culture.Equals(Cultures.GermanNeutral))
            {
                countryCode = CountryCodes.Germany;
                return true;
            }

            if (culture.Equals(Cultures.GermanAustria))
            {
                countryCode = CountryCodes.Austria;
                return true;
            }

            if (culture.Equals(Cultures.FrenchSwitzerland) ||
                culture.Equals(Cultures.GermanSwitzerland) ||
                culture.Equals(Cultures.ItalianSwitzerland) ||
                culture.Equals(Cultures.RomanshSwitzerland))
            {
                countryCode = CountryCodes.Switzerland;
                return true;
            }

            if (culture.Equals(Cultures.FrenchFrance) || 
                culture.Equals(Cultures.FrenchNeutral))
            {
                countryCode = CountryCodes.France;
                return true;
            }

            countryCode = CountryCode.Empty;
            return false;
        }

        public static bool TryParse(string s, out CountryCode countryCode)
        {
            if (string.IsNullOrEmpty(s))
            {
                countryCode = CountryCode.Empty;
                return false;
            }

            string numbers = new string(s.ToCharArray().Where(char.IsDigit).ToArray());

            ushort cc;
            if (ushort.TryParse(numbers, out cc))
            {
                countryCode = new CountryCode(cc);
                return true;
            }

            countryCode = CountryCode.Empty;
            return false;
        }

        public int CompareTo(CountryCode other)
        {
            if (other == null)
            {
                return 1;
            }

            return this.countryCode.CompareTo(other.countryCode);
        }

        public bool Equals(CountryCode other)
        {
            if (other == null)
            {
                return false;
            }

            return this.countryCode == other.countryCode;
        }

        public override bool Equals(object obj)
        {
            CountryCode other = obj as CountryCode;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.countryCode.GetHashCode();
        }

        public string ToString(string format)
        {
            return this.ToString(format, CultureInfo.CurrentCulture);
        }

        public string ToString(IFormatProvider formatProvider)
        {
            return this.ToString(string.Empty, formatProvider);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (this.countryCode <= 0)
            {
                return string.Empty;
            }

            format = (format ?? string.Empty).ToUpperInvariant();

            formatProvider = formatProvider ?? CultureInfo.CurrentCulture;

            switch (format)
            {
                case FormatStrings.PhoneNumbers.Number:
                {
                    return this.countryCode.ToString();
                }

                case FormatStrings.PhoneNumbers.General:
                {
                    return "+" + this.countryCode;
                }

                default:
                {
                    throw new FormatException();
                }
            }
        }

        public override string ToString()
        {
            return this.ToString(FormatStrings.PhoneNumbers.General, CultureInfo.CurrentCulture);
        }
    }
}