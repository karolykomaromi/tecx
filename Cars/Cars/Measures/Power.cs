namespace Cars.Measures
{
    using System;
    using System.Diagnostics.Contracts;

    public struct Power : IEquatable<Power>, IComparable<Power>
    {
        public static readonly Power Zero = new Power(0);

        private readonly decimal powerInWatts;

        private Power(decimal powerInWatts)
        {
            this.powerInWatts = powerInWatts;
        }

        public static Power operator +(Power x, Power y)
        {
            return new Power(x.powerInWatts + y.powerInWatts);
        }

        public static Power operator -(Power x, Power y)
        {
            return new Power(x.powerInWatts - y.powerInWatts);
        }

        public static bool operator ==(Power x, Power y)
        {
            return x.powerInWatts == y.powerInWatts;
        }

        public static bool operator !=(Power x, Power y)
        {
            return !(x == y);
        }

        public static bool operator <(Power x, Power y)
        {
            return x.powerInWatts < y.powerInWatts;
        }

        public static bool operator >(Power x, Power y)
        {
            return x.powerInWatts > y.powerInWatts;
        }

        public static bool operator <=(Power x, Power y)
        {
            return x.powerInWatts <= y.powerInWatts;
        }

        public static bool operator >=(Power x, Power y)
        {
            return x.powerInWatts >= y.powerInWatts;
        }

        public static Power operator /(Power power, int divisor)
        {
            Contract.Requires(power != null);
            Contract.Ensures(Contract.Result<Power>() != null);

            return new Power(power.powerInWatts / divisor);
        }

        public static Power operator *(Power power, int factor)
        {
            Contract.Requires(power != null);
            Contract.Ensures(Contract.Result<Power>() != null);

            return new Power(power.powerInWatts * factor);
        }

        public static Power operator *(int factor, Power power)
        {
            Contract.Requires(power != null);
            Contract.Ensures(Contract.Result<Power>() != null);

            return new Power(power.powerInWatts * factor);
        }

        public static Power operator /(Power power, long divisor)
        {
            Contract.Requires(power != null);
            Contract.Ensures(Contract.Result<Power>() != null);

            return new Power(power.powerInWatts / divisor);
        }

        public static Power operator *(Power power, long factor)
        {
            Contract.Requires(power != null);
            Contract.Ensures(Contract.Result<Power>() != null);

            return new Power(power.powerInWatts * factor);
        }

        public static Power operator *(long factor, Power power)
        {
            Contract.Requires(power != null);
            Contract.Ensures(Contract.Result<Power>() != null);

            return new Power(power.powerInWatts * factor);
        }

        public static Power operator /(Power power, double divisor)
        {
            Contract.Requires(power != null);
            Contract.Ensures(Contract.Result<Power>() != null);

            return new Power(power.powerInWatts / new decimal(divisor));
        }

        public static Power operator *(Power power, double factor)
        {
            Contract.Requires(power != null);
            Contract.Ensures(Contract.Result<Power>() != null);

            return new Power(power.powerInWatts * new decimal(factor));
        }

        public static Power operator *(double factor, Power power)
        {
            Contract.Requires(power != null);
            Contract.Ensures(Contract.Result<Power>() != null);

            return new Power(power.powerInWatts * new decimal(factor));
        }

        public static Power operator /(Power power, decimal divisor)
        {
            Contract.Requires(power != null);
            Contract.Ensures(Contract.Result<Power>() != null);

            return new Power(power.powerInWatts / divisor);
        }

        public static Power operator *(Power power, decimal factor)
        {
            Contract.Requires(power != null);
            Contract.Ensures(Contract.Result<Power>() != null);

            return new Power(power.powerInWatts * factor);
        }

        public static Power operator *(decimal factor, Power power)
        {
            Contract.Requires(power != null);
            Contract.Ensures(Contract.Result<Power>() != null);

            return new Power(power.powerInWatts * factor);
        }

        public static Power FromWatts(decimal powerInWatts)
        {
            return new Power(powerInWatts);
        }

        public static Power FromKiloWatts(decimal powerInKiloWatts)
        {
            return new Power(powerInKiloWatts * 1000);
        }

        public int CompareTo(Power other)
        {
            return this.powerInWatts.CompareTo(other.powerInWatts);
        }

        public bool Equals(Power other)
        {
            return this.powerInWatts == other.powerInWatts;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Power))
            {
                return false;
            }

            Power other = (Power)obj;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.powerInWatts.GetHashCode();
        }
    }
}