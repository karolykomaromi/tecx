namespace Cars.Measures
{
    using System;
    using System.Diagnostics.Contracts;

    public struct Distance : IEquatable<Distance>, IComparable<Distance>
    {
        public static readonly Distance Zero = new Distance(0);

        private readonly decimal distanceInMeters;

        private Distance(decimal distanceInMeters)
        {
            this.distanceInMeters = distanceInMeters;
        }

        public static Distance operator +(Distance x, Distance y)
        {
            return new Distance(checked(x.distanceInMeters + y.distanceInMeters));
        }

        public static Distance operator -(Distance x, Distance y)
        {
            return new Distance(checked(x.distanceInMeters - y.distanceInMeters));
        }

        public static bool operator ==(Distance x, Distance y)
        {
            return x.distanceInMeters == y.distanceInMeters;
        }

        public static bool operator !=(Distance x, Distance y)
        {
            return !(x == y);
        }

        public static bool operator <(Distance x, Distance y)
        {
            return x.distanceInMeters < y.distanceInMeters;
        }

        public static bool operator >(Distance x, Distance y)
        {
            return x.distanceInMeters > y.distanceInMeters;
        }

        public static bool operator <=(Distance x, Distance y)
        {
            return x.distanceInMeters <= y.distanceInMeters;
        }

        public static bool operator >=(Distance x, Distance y)
        {
            return x.distanceInMeters >= y.distanceInMeters;
        }

        public static Distance operator /(Distance distance, int divisor)
        {
            Contract.Requires(distance != null);
            Contract.Ensures(Contract.Result<Distance>() != null);

            return new Distance(distance.distanceInMeters / divisor);
        }

        public static Distance operator *(Distance distance, int factor)
        {
            Contract.Requires(distance != null);
            Contract.Ensures(Contract.Result<Distance>() != null);

            return new Distance(distance.distanceInMeters * factor);
        }

        public static Distance operator *(int factor, Distance distance)
        {
            Contract.Requires(distance != null);
            Contract.Ensures(Contract.Result<Distance>() != null);

            return new Distance(distance.distanceInMeters * factor);
        }

        public static Distance operator /(Distance distance, long divisor)
        {
            Contract.Requires(distance != null);
            Contract.Ensures(Contract.Result<Distance>() != null);

            return new Distance(distance.distanceInMeters / divisor);
        }

        public static Distance operator *(Distance distance, long factor)
        {
            Contract.Requires(distance != null);
            Contract.Ensures(Contract.Result<Distance>() != null);

            return new Distance(distance.distanceInMeters * factor);
        }

        public static Distance operator *(long factor, Distance distance)
        {
            Contract.Requires(distance != null);
            Contract.Ensures(Contract.Result<Distance>() != null);

            return new Distance(distance.distanceInMeters * factor);
        }

        public static Distance operator /(Distance distance, double divisor)
        {
            Contract.Requires(distance != null);
            Contract.Ensures(Contract.Result<Distance>() != null);

            return new Distance(distance.distanceInMeters / new decimal(divisor));
        }

        public static Distance operator *(Distance distance, double factor)
        {
            Contract.Requires(distance != null);
            Contract.Ensures(Contract.Result<Distance>() != null);

            return new Distance(distance.distanceInMeters * new decimal(factor));
        }

        public static Distance operator *(double factor, Distance distance)
        {
            Contract.Requires(distance != null);
            Contract.Ensures(Contract.Result<Distance>() != null);

            return new Distance(distance.distanceInMeters * new decimal(factor));
        }

        public static Distance operator /(Distance distance, decimal divisor)
        {
            Contract.Requires(distance != null);
            Contract.Ensures(Contract.Result<Distance>() != null);

            return new Distance(distance.distanceInMeters / divisor);
        }

        public static Distance operator *(Distance distance, decimal factor)
        {
            Contract.Requires(distance != null);
            Contract.Ensures(Contract.Result<Distance>() != null);

            return new Distance(distance.distanceInMeters * factor);
        }

        public static Distance operator *(decimal factor, Distance distance)
        {
            Contract.Requires(distance != null);
            Contract.Ensures(Contract.Result<Distance>() != null);

            return new Distance(distance.distanceInMeters * factor);
        }

        public static Distance FromMeters(decimal distanceInMeters)
        {
            return new Distance(distanceInMeters);
        }

        public static Distance FromMillimeters(decimal distanceInMillimeters)
        {
            return new Distance(distanceInMillimeters / 1000);
        }

        public static Distance FromCentimeters(decimal distanceInCentimeters)
        {
            return new Distance(distanceInCentimeters / 100);
        }

        public static Distance FromDecimeters(decimal distanceInDecimeters)
        {
            return new Distance(distanceInDecimeters / 10);
        }

        public static Distance FromKilometers(decimal distanceInKilometers)
        {
            return new Distance(distanceInKilometers * 1000);
        }

        public int CompareTo(Distance other)
        {
            return this.distanceInMeters.CompareTo(other.distanceInMeters);
        }

        public bool Equals(Distance other)
        {
            return this.distanceInMeters == other.distanceInMeters;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Distance))
            {
                return false;
            }

            Distance other = (Distance)obj;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.distanceInMeters.GetHashCode();
        }
    }
}