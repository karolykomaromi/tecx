using System;
using System.Diagnostics;
using System.Globalization;

using TecX.Common.Comparison;

namespace TecX.Common.Measures
{
    [DebuggerDisplay("{_value} km")]
    public struct Kilometer : IFormattable
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

        public static Kilometer operator +(Kilometer distance1, Meter distance2)
        {
            return new Kilometer(distance1._value + distance2.ToKilometers()._value);
        }

        public static Kilometer operator +(Kilometer distance1, double distance2)
        {
            return new Kilometer(distance1._value + distance2);
        }

        public static Kilometer operator -(Kilometer distance1, Kilometer distance2)
        {
            return new Kilometer(distance1._value - distance2._value);
        }

        public static Kilometer operator -(Kilometer distance1, Meter distance2)
        {
            return new Kilometer(distance1._value - distance2.ToKilometers()._value);
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

        public Meter ToMeters()
        {
            return new Meter(_value * 1000.0);
        }

        public override string ToString()
        {
            return this.ToString("F3", CultureInfo.InvariantCulture);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return _value.ToString(format, formatProvider) + " km";
        }
    }
}
