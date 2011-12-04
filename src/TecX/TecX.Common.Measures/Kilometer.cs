namespace TecX.Common.Measures
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    using TecX.Common.Comparison;

    [DebuggerDisplay("{value} km")]
    public struct Kilometer : IFormattable
    {
        private readonly double value;

        public Kilometer(double value)
        {
            this.value = value;
        }

        public static implicit operator Kilometer(double value)
        {
            return new Kilometer(value);
        }

        public static explicit operator double(Kilometer distance)
        {
            return distance.value;
        }

        public static Kilometer operator +(Kilometer distance1, Kilometer distance2)
        {
            return new Kilometer(distance1.value + distance2.value);
        }

        public static Kilometer operator +(Kilometer distance1, Meter distance2)
        {
            return new Kilometer(distance1.value + distance2.ToKilometers().value);
        }

        public static Kilometer operator +(Kilometer distance1, double distance2)
        {
            return new Kilometer(distance1.value + distance2);
        }

        public static Kilometer operator -(Kilometer distance1, Kilometer distance2)
        {
            return new Kilometer(distance1.value - distance2.value);
        }

        public static Kilometer operator -(Kilometer distance1, Meter distance2)
        {
            return new Kilometer(distance1.value - distance2.ToKilometers().value);
        }

        public static Kilometer operator -(Kilometer distance1, double distance2)
        {
            return new Kilometer(distance1.value - distance2);
        }

        public static bool operator ==(Kilometer distance1, Kilometer distance2)
        {
            return EpsilonComparer.AreEqual(distance1.value, distance2.value);
        }

        public static bool operator !=(Kilometer distance1, Kilometer distance2)
        {
            return !EpsilonComparer.AreEqual(distance1.value, distance2.value);
        }

        public static KilometersPerHour operator /(Kilometer distance, TimeSpan time)
        {
            return new KilometersPerHour(distance.value / time.TotalHours);
        }

        public Meter ToMeters()
        {
            return new Meter(this.value * 1000.0);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Kilometer))
            {
                return false;
            }

            return ((Kilometer)obj) == this;
        }

        public override int GetHashCode()
        {
            return (int)this.value;
        }

        public override string ToString()
        {
            return this.ToString("F3", CultureInfo.InvariantCulture);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return this.value.ToString(format, formatProvider) + " km";
        }
    }
}
