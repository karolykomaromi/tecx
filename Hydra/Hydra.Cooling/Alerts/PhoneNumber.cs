namespace Hydra.Cooling.Alerts
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;

    public class PhoneNumber : IEquatable<PhoneNumber>, IFormattable
    {
        public static readonly PhoneNumber Empty = new PhoneNumber();

        private readonly CountryCode countryCode;

        private readonly AreaCode areaCode;

        private readonly DialNumber dialNumber;

        private readonly PhoneExtension phoneExtension;

        public PhoneNumber(
            CountryCode countryCode,
            AreaCode areaCode,
            DialNumber dialNumber)
            : this(countryCode, areaCode, dialNumber, PhoneExtension.Empty)
        {
        }

        public PhoneNumber(
            CountryCode countryCode,
            AreaCode areaCode,
            DialNumber dialNumber,
            PhoneExtension phoneExtension)
        {
            Contract.Requires(countryCode != null);
            Contract.Requires(countryCode != CountryCode.Empty);
            Contract.Requires(areaCode != null);
            Contract.Requires(areaCode != AreaCode.Empty);
            Contract.Requires(dialNumber != null);
            Contract.Requires(dialNumber != DialNumber.Empty);
            Contract.Requires(phoneExtension != null);

            this.countryCode = countryCode;
            this.areaCode = areaCode;
            this.dialNumber = dialNumber;
            this.phoneExtension = phoneExtension;
        }

        private PhoneNumber()
        {
            this.countryCode = CountryCode.Empty;
            this.areaCode = AreaCode.Empty;
            this.dialNumber = DialNumber.Empty;
            this.phoneExtension = PhoneExtension.Empty;
        }

        public CountryCode CountryCode
        {
            get
            {
                Contract.Ensures(Contract.Result<CountryCode>() != null);

                return this.countryCode;
            }
        }

        public AreaCode AreaCode
        {
            get
            {
                Contract.Ensures(Contract.Result<AreaCode>() != null);

                return this.areaCode;
            }
        }

        public DialNumber DialNumber
        {
            get
            {
                Contract.Ensures(Contract.Result<DialNumber>() != null);

                return this.dialNumber;
            }
        }

        public PhoneExtension PhoneExtension
        {
            get
            {
                Contract.Ensures(Contract.Result<PhoneExtension>() != null);

                return this.phoneExtension;
            }
        }

        public static bool operator ==(PhoneNumber first, PhoneNumber second)
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

        public static bool operator !=(PhoneNumber first, PhoneNumber second)
        {
            return !(first == second);
        }

        public bool Equals(PhoneNumber other)
        {
            if (other == null)
            {
                return false;
            }

            if (!this.CountryCode.Equals(other.CountryCode))
            {
                return false;
            }

            if (!this.AreaCode.Equals(other.AreaCode))
            {
                return false;
            }

            if (!this.DialNumber.Equals(other.DialNumber))
            {
                return false;
            }

            if (!this.PhoneExtension.Equals(other.PhoneExtension))
            {
                return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            PhoneNumber other = obj as PhoneNumber;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.CountryCode.GetHashCode() ^
                   this.AreaCode.GetHashCode() ^
                   this.DialNumber.GetHashCode() ^
                   this.PhoneExtension.GetHashCode();
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
            format = (format ?? string.Empty).ToUpperInvariant();
            formatProvider = formatProvider ?? CultureInfo.CurrentCulture;

            switch (format)
            {
                case FormatStrings.PhoneNumbers.General:
                {
                    var parts = new[]
                    {
                        this.CountryCode.ToString(FormatStrings.PhoneNumbers.General, formatProvider),
                        this.AreaCode.ToString(FormatStrings.PhoneNumbers.General, formatProvider),
                        this.DialNumber.ToString(FormatStrings.PhoneNumbers.General, formatProvider),
                        this.PhoneExtension.ToString(FormatStrings.PhoneNumbers.General, formatProvider)
                    }.Where(s => !string.IsNullOrWhiteSpace(s));

                    string phoneNumber = string.Join(" ", parts);

                    return phoneNumber.Trim();
                }

                case FormatStrings.PhoneNumbers.Number:
                {
                    var parts = new[]
                    {
                        this.CountryCode.ToString(FormatStrings.PhoneNumbers.Number, formatProvider),
                        this.AreaCode.ToString(FormatStrings.PhoneNumbers.Number, formatProvider),
                        this.DialNumber.ToString(FormatStrings.PhoneNumbers.Number, formatProvider),
                        this.PhoneExtension.ToString(FormatStrings.PhoneNumbers.Number, formatProvider)
                    }.Where(s => !string.IsNullOrWhiteSpace(s));

                    string phoneNumber = string.Join(string.Empty, parts);

                    return phoneNumber.Trim();
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