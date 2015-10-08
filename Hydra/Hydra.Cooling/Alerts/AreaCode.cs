namespace Hydra.Cooling.Alerts
{
    using System;
    using System.Globalization;

    public class AreaCode : IEquatable<AreaCode>, IComparable<AreaCode>, IFormattable
    {
        public static readonly AreaCode Empty = new AreaCode();

        private readonly uint areaCode;

        public AreaCode(uint areaCode)
        {
            this.areaCode = areaCode;
        }

        private AreaCode()
        {
        }

        public static bool operator ==(AreaCode first, AreaCode second)
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

        public static bool operator !=(AreaCode first, AreaCode second)
        {
            return !(first == second);
        }

        public int CompareTo(AreaCode other)
        {
            if (other == null)
            {
                return 1;
            }

            return this.areaCode.CompareTo(other.areaCode);
        }

        public bool Equals(AreaCode other)
        {
            if (other == null)
            {
                return false;
            }

            return this.areaCode == other.areaCode;
        }

        public override bool Equals(object obj)
        {
            AreaCode other = obj as AreaCode;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.areaCode.GetHashCode();
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
            if (this.areaCode <= 0)
            {
                return string.Empty;
            }

            format = (format ?? string.Empty).ToUpperInvariant();
            formatProvider = formatProvider ?? CultureInfo.CurrentCulture;

            switch (format)
            {
                case FormatStrings.PhoneNumbers.General:
                {
                    return "(" + this.areaCode + ")";
                }

                case FormatStrings.PhoneNumbers.Number:
                {
                    return this.areaCode.ToString();
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
