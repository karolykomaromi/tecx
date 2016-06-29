namespace Cars.Measures
{
    using System;
    using System.Diagnostics.Contracts;

    public class Distance : IEquatable<Distance>, IComparable<Distance>
    {
        private readonly decimal distanceInMeters;

        private Distance(decimal distanceInMeters)
        {
            this.distanceInMeters = distanceInMeters;
        }

        public static Distance operator +(Distance x, Distance y)
        {
            Contract.Requires(x != null);
            Contract.Requires(y != null);
            Contract.Ensures(Contract.Result<Weight>() != null);

            return new Distance(x.distanceInMeters + y.distanceInMeters);
        }

        public static Distance operator -(Distance x, Distance y)
        {
            Contract.Requires(x != null);
            Contract.Requires(y != null);
            Contract.Ensures(Contract.Result<Distance>() != null);

            return new Distance(x.distanceInMeters - y.distanceInMeters);
        }

        public static bool operator ==(Distance x, Distance y)
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

            return x.distanceInMeters == y.distanceInMeters;
        }

        public static bool operator !=(Distance x, Distance y)
        {
            return !(x == y);
        }

        public static bool operator <(Distance x, Distance y)
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

            return x.distanceInMeters < y.distanceInMeters;
        }

        public static bool operator >(Distance x, Distance y)
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

            return x.distanceInMeters > y.distanceInMeters;
        }

        public static bool operator <=(Distance x, Distance y)
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

            return x.distanceInMeters <= y.distanceInMeters;
        }

        public static bool operator >=(Distance x, Distance y)
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
            if (object.ReferenceEquals(other, null))
            {
                return 1;
            }

            return this.distanceInMeters.CompareTo(other.distanceInMeters);
        }

        public bool Equals(Distance other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }

            return this.distanceInMeters == other.distanceInMeters;
        }

        public override bool Equals(object obj)
        {
            Distance other = obj as Distance;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.distanceInMeters.GetHashCode();
        }
    }

    public static class DistanceExtensions
    {
        public static Distance Kilometers(this double kilometers)
        {
            return Distance.FromKilometers(new decimal(kilometers));
        }

        public static Distance Kilometers(this decimal kilometers)
        {
            return Distance.FromKilometers(kilometers);
        }

        public static Distance Centimeters(this double centimeters)
        {
            return Distance.FromCentimeters(new decimal(centimeters));
        }

        public static Distance Centimeters(this decimal centimeters)
        {
            return Distance.FromCentimeters(centimeters);
        }
        
        public static Distance Millimeters(this double millimeters)
        {
            return Distance.FromMillimeters(new decimal(millimeters));
        }

        public static Distance Millimeters(this decimal millimeters)
        {
            return Distance.FromMillimeters(millimeters);
        }

        public static Distance Meters(this double meters)
        {
            return Distance.FromMeters(new decimal(meters));
        }

        public static Distance Meters(this decimal meters)
        {
            return Distance.FromMeters(meters);
        }
    }
}