namespace Cars.Measures
{
    using System;
    using System.Diagnostics.Contracts;

    public struct Volume : IEquatable<Volume>, IComparable<Volume>
    {
        public static readonly Volume Zero = new Volume(0);

        private readonly decimal volumeInLiters;

        private Volume(decimal volumeInLiters)
        {
            this.volumeInLiters = volumeInLiters;
        }

        public static Volume operator +(Volume v1, Volume v2)
        {
            return new Volume(v1.volumeInLiters + v2.volumeInLiters);
        }

        public static Volume operator -(Volume v1, Volume v2)
        {
            return new Volume(v1.volumeInLiters - v2.volumeInLiters);
        }

        public static bool operator ==(Volume x, Volume y)
        {
            return x.volumeInLiters == y.volumeInLiters;
        }

        public static bool operator !=(Volume x, Volume y)
        {
            return !(x == y);
        }

        public static bool operator <(Volume x, Volume y)
        {
            return x.volumeInLiters < y.volumeInLiters;
        }

        public static bool operator >(Volume x, Volume y)
        {
            return x.volumeInLiters > y.volumeInLiters;
        }

        public static bool operator <=(Volume x, Volume y)
        {
            return x.volumeInLiters <= y.volumeInLiters;
        }

        public static bool operator >=(Volume x, Volume y)
        {
            return x.volumeInLiters >= y.volumeInLiters;
        }

        public static Volume operator /(Volume volume, int divisor)
        {
            Contract.Requires(volume != null);
            Contract.Ensures(Contract.Result<Volume>() != null);

            return new Volume(volume.volumeInLiters / divisor);
        }

        public static Volume operator *(Volume volume, int factor)
        {
            Contract.Requires(volume != null);
            Contract.Ensures(Contract.Result<Volume>() != null);

            return new Volume(volume.volumeInLiters * factor);
        }

        public static Volume operator *(int factor, Volume volume)
        {
            Contract.Requires(volume != null);
            Contract.Ensures(Contract.Result<Volume>() != null);

            return new Volume(volume.volumeInLiters * factor);
        }

        public static Volume operator /(Volume volume, long divisor)
        {
            Contract.Requires(volume != null);
            Contract.Ensures(Contract.Result<Volume>() != null);

            return new Volume(volume.volumeInLiters / divisor);
        }

        public static Volume operator *(Volume volume, long factor)
        {
            Contract.Requires(volume != null);
            Contract.Ensures(Contract.Result<Volume>() != null);

            return new Volume(volume.volumeInLiters * factor);
        }

        public static Volume operator *(long factor, Volume volume)
        {
            Contract.Requires(volume != null);
            Contract.Ensures(Contract.Result<Volume>() != null);

            return new Volume(volume.volumeInLiters * factor);
        }

        public static Volume operator /(Volume volume, double divisor)
        {
            Contract.Requires(volume != null);
            Contract.Ensures(Contract.Result<Volume>() != null);

            return new Volume(volume.volumeInLiters / new decimal(divisor));
        }

        public static Volume operator *(Volume volume, double factor)
        {
            Contract.Requires(volume != null);
            Contract.Ensures(Contract.Result<Volume>() != null);

            return new Volume(volume.volumeInLiters * new decimal(factor));
        }

        public static Volume operator *(double factor, Volume volume)
        {
            Contract.Requires(volume != null);
            Contract.Ensures(Contract.Result<Volume>() != null);

            return new Volume(volume.volumeInLiters * new decimal(factor));
        }

        public static Volume operator /(Volume volume, decimal divisor)
        {
            Contract.Requires(volume != null);
            Contract.Ensures(Contract.Result<Volume>() != null);

            return new Volume(volume.volumeInLiters / divisor);
        }

        public static Volume operator *(Volume volume, decimal factor)
        {
            Contract.Requires(volume != null);
            Contract.Ensures(Contract.Result<Volume>() != null);

            return new Volume(volume.volumeInLiters * factor);
        }

        public static Volume operator *(decimal factor, Volume volume)
        {
            Contract.Requires(volume != null);
            Contract.Ensures(Contract.Result<Volume>() != null);

            return new Volume(volume.volumeInLiters * factor);
        }

        public static Volume FromLiters(decimal volumeInLiters)
        {
            return new Volume(volumeInLiters);
        }

        public static Volume FromMilliliters(decimal volumeInMilliliters)
        {
            return new Volume(volumeInMilliliters / 1000);
        }

        public static Volume FromCubicCentimeters(decimal volumeInCubicCentimeters)
        {
            return new Volume(volumeInCubicCentimeters / 1000);
        }

        public int CompareTo(Volume other)
        {
            return this.volumeInLiters.CompareTo(other.volumeInLiters);
        }

        public bool Equals(Volume other)
        {
            return this.volumeInLiters == other.volumeInLiters;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Volume))
            {
                return false;
            }

            Volume other = (Volume)obj;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.volumeInLiters.GetHashCode();
        }
    }
}