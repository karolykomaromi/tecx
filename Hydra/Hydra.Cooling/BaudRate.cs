namespace Hydra.Cooling
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class BaudRate : IEquatable<BaudRate>, IComparable<BaudRate>
    {
        public static readonly BaudRate Bd19200 = new BaudRate();

        public static readonly BaudRate Bd9600 = new BaudRate();

        private readonly int rate;

        private BaudRate([CallerMemberName] string name = null)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));

            string s = new string((name ?? string.Empty).ToCharArray().Where(char.IsDigit).ToArray());

            this.rate = int.Parse(s, NumberStyles.Integer);
        }

        public static implicit operator int(BaudRate bd)
        {
            Contract.Requires(bd != null);

            return bd.rate;
        }

        public static bool operator ==(BaudRate x, BaudRate y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return true;
            }

            if (object.ReferenceEquals(x, null))
            {
                return false;
            }

            if (object.ReferenceEquals(y, null))
            {
                return false;
            }

            return x.Equals(y);
        }

        public static bool operator !=(BaudRate x, BaudRate y)
        {
            return !(x == y);
        }

        public static bool operator <(BaudRate x, BaudRate y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return false;
            }

            if (object.ReferenceEquals(x, null))
            {
                return true;
            }

            if (object.ReferenceEquals(y, null))
            {
                return false;
            }

            return x.rate < y.rate;
        }

        public static bool operator <=(BaudRate x, BaudRate y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return true;
            }

            if (object.ReferenceEquals(x, null))
            {
                return true;
            }

            if (object.ReferenceEquals(y, null))
            {
                return false;
            }

            return x.rate <= y.rate;
        }

        public static bool operator >(BaudRate x, BaudRate y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return false;
            }

            if (object.ReferenceEquals(x, null))
            {
                return false;
            }

            if (object.ReferenceEquals(y, null))
            {
                return true;
            }

            return x.rate > y.rate;
        }

        public static bool operator >=(BaudRate x, BaudRate y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return true;
            }

            if (object.ReferenceEquals(x, null))
            {
                return false;
            }

            if (object.ReferenceEquals(y, null))
            {
                return true;
            }

            return x.rate >= y.rate;
        }

        public bool Equals(BaudRate other)
        {
            if (other == null)
            {
                return false;
            }

            return this.rate.Equals(other.rate);
        }

        public override bool Equals(object obj)
        {
            BaudRate other = obj as BaudRate;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.rate;
        }

        public int CompareTo(BaudRate other)
        {
            if (other == null)
            {
                return 1;
            }

            return this.rate.CompareTo(other.rate);
        }

        public override string ToString()
        {
            return this.rate.ToString();
        }
    }
}