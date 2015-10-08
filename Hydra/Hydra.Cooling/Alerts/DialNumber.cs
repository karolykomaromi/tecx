namespace Hydra.Cooling.Alerts
{
    using System;
    using System.Globalization;

    public class DialNumber : IEquatable<DialNumber>, IComparable<DialNumber>, IFormattable
    {
        public static readonly DialNumber Empty = new DialNumber();

        private readonly ulong dialNumber;

        public DialNumber(ulong dialNumber)
        {
            this.dialNumber = dialNumber;
        }

        private DialNumber()
        {
        }

        public static bool operator ==(DialNumber first, DialNumber second)
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

        public static bool operator !=(DialNumber first, DialNumber second)
        {
            return !(first == second);
        }

        public int CompareTo(DialNumber other)
        {
            if (other == null)
            {
                return 1;
            }

            return this.dialNumber.CompareTo(other.dialNumber);
        }

        public bool Equals(DialNumber other)
        {
            if (other == null)
            {
                return false;
            }

            return this.dialNumber == other.dialNumber;
        }

        public override bool Equals(object obj)
        {
            DialNumber other = obj as DialNumber;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.dialNumber.GetHashCode();
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
            if (this.dialNumber <= 0)
            {
                return string.Empty;
            }

            format = (format ?? string.Empty).ToUpperInvariant();
            formatProvider = formatProvider ?? CultureInfo.CurrentCulture;

            switch (format)
            {
                case FormatStrings.PhoneNumbers.General:
                {
                    // SWE 2015-10-08 Chunkify 3 or 4?
                    return this.dialNumber.ToString();
                }

                case FormatStrings.PhoneNumbers.Number:
                {
                    return this.dialNumber.ToString();
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