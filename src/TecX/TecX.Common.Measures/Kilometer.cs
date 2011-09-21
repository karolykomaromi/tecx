using System;
using TecX.Common.Comparison;

namespace TecX.Common.Measures
{
    public struct Kilometer
    {
        private readonly double _value;

        public Kilometer(double value)
        {
            _value = value;
        }

        public static implicit operator Kilometer(double value)
        {
            return new Kilometer(value);
        }

        public static explicit operator double(Kilometer distance)
        {
            return distance._value;
        }

        public static Kilometer operator +(Kilometer distance1, Kilometer distance2)
        {
            return new Kilometer(distance1._value + distance2._value);
        }

        public static Kilometer operator +(Kilometer distance1, double distance2)
        {
            return new Kilometer(distance1._value + distance2);
        }

        public static Kilometer operator -(Kilometer distance1, Kilometer distance2)
        {
            return new Kilometer(distance1._value - distance2._value);
        }

        public static Kilometer operator -(Kilometer distance1, double distance2)
        {
            return new Kilometer(distance1._value - distance2);
        }

        public static bool operator ==(Kilometer distance1, Kilometer distance2)
        {
            return EpsilonComparer.AreEqual(distance1._value, distance2._value);
        }

        public static bool operator !=(Kilometer distance1, Kilometer distance2)
        {
            return !EpsilonComparer.AreEqual(distance1._value, distance2._value);
        }

        public static KilometersPerHour operator /(Kilometer distance, TimeSpan time)
        {
            return new KilometersPerHour(distance._value / time.TotalHours);
        }
    }
}
