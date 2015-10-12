namespace Hydra.Cooling.Alerts
{
    using System;
    using System.Globalization;
    using System.Linq;

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

        public static bool TryParse(string s, out AreaCode areaCode)
        {
            if (string.IsNullOrEmpty(s))
            {
                areaCode = AreaCode.Empty;
                return false;
            }

            string numbers = new string(s.ToCharArray().Where(char.IsDigit).ToArray());

            uint ac;
            if (uint.TryParse(numbers, out ac))
            {
                areaCode = new AreaCode(ac);
                return true;
            }

            areaCode = AreaCode.Empty;
            return false;
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
