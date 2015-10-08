namespace Hydra.Cooling.Alerts
{
    using System;
    using System.Globalization;

    public class PhoneExtension : IEquatable<PhoneExtension>, IComparable<PhoneExtension>, IFormattable
    {
        public static readonly PhoneExtension Empty = new PhoneExtension();

        private readonly uint phoneExtension;

        public PhoneExtension(uint phoneExtension)
        {
            this.phoneExtension = phoneExtension;
        }

        private PhoneExtension()
        {
        }

        public static bool operator ==(PhoneExtension first, PhoneExtension second)
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

        public static bool operator !=(PhoneExtension first, PhoneExtension second)
        {
            return !(first == second);
        }

        public int CompareTo(PhoneExtension other)
        {
            if (other == null)
            {
                return 1;
            }

            return this.phoneExtension.CompareTo(other.phoneExtension);
        }

        public bool Equals(PhoneExtension other)
        {
            if (other == null)
            {
                return false;
            }

            return this.phoneExtension == other.phoneExtension;
        }

        public override bool Equals(object obj)
        {
            PhoneExtension other = obj as PhoneExtension;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.phoneExtension.GetHashCode();
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
            if (this.phoneExtension <= 0)
            {
                return string.Empty;
            }

            format = (format ?? string.Empty).ToUpperInvariant();
            formatProvider = formatProvider ?? CultureInfo.CurrentCulture;

            switch (format)
            {
                case FormatStrings.PhoneNumbers.General:
                {
                    return "- " + this.phoneExtension;
                }

                case FormatStrings.PhoneNumbers.Number:
                {
                    return this.phoneExtension.ToString();
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