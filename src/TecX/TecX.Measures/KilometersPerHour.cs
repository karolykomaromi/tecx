namespace TecX.Measures
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    using TecX.Common.Comparison;

    [DebuggerDisplay("{value} km/h")]
    public struct KilometersPerHour : IFormattable
    {
        private readonly double value;

        public KilometersPerHour(double value)
        {
            this.value = value;
        }

        public static implicit operator KilometersPerHour(double value)
        {
            return new KilometersPerHour(value);
        }

        public static explicit operator double(KilometersPerHour speed)
        {
            return speed.value;
        }

        public static KilometersPerHour operator +(KilometersPerHour speed1, KilometersPerHour speed2)
        {
            return new KilometersPerHour(speed1.value + speed2.value);
        }

        public static KilometersPerHour operator +(KilometersPerHour speed1, double speed2)
        {
            return new KilometersPerHour(speed1.value + speed2);
        }

        public static KilometersPerHour operator -(KilometersPerHour speed1, KilometersPerHour speed2)
        {
            return new KilometersPerHour(speed1.value - speed2.value);
        }

        public static KilometersPerHour operator -(KilometersPerHour speed1, double speed2)
        {
            return new KilometersPerHour(speed1.value - speed2);
        }

        public static bool operator ==(KilometersPerHour speed1, KilometersPerHour speed2)
        {
            return EpsilonComparer.AreEqual(speed1.value, speed2.value);
        }

        public static bool operator !=(KilometersPerHour speed1, KilometersPerHour speed2)
        {
            return !EpsilonComparer.AreEqual(speed1.value, speed2.value);
        }

        public static Kilometer operator *(KilometersPerHour speed, TimeSpan time)
        {
            return speed.value * time.TotalHours;
        }

        public static KilometersPerHour FromDistanceAndTime(double distanceInKilometers, TimeSpan time)
        {
            return new KilometersPerHour(distanceInKilometers / time.TotalHours);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is KilometersPerHour))
            {
                return false;
            }

            return ((KilometersPerHour)obj) == this;
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
            return this.value.ToString(format, formatProvider) + " km/h";
        }
    }
}
