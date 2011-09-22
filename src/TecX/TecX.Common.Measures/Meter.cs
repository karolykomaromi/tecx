using System;
using System.Diagnostics;
using System.Globalization;

namespace TecX.Common.Measures
{
    [DebuggerDisplay("{_value,{F2},{en-US}} m")]
    public struct Meter : IFormattable
    {
        private readonly double _value;

        public Meter(double value)
        {
            this._value = value;
        }

        public static implicit operator Meter(double distance)
        {
            return new Meter(distance);
        }

        public static explicit operator double(Meter distance)
        {
            return distance._value;
        }

        public static Meter operator +(Meter distance1, Meter distance2)
        {
            return new Meter(distance1._value + distance2._value);
        }

        public static Meter operator +(Meter distance1, double distance2)
        {
            return new Meter(distance1._value + distance2);
        }

        public static Meter operator -(Meter distance1, Meter distance2)
        {
            return new Meter(distance1._value - distance2._value);
        }

        public static Meter operator -(Meter distance1, double distance2)
        {
            return new Meter(distance1._value - distance2);
        }

        public Kilometer ToKilometers()
        {
            return new Kilometer(_value / 1000.0);
        }

        public override string ToString()
        {
            return this.ToString("F2", CultureInfo.InvariantCulture);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return _value.ToString(format, formatProvider) + " m";
        }
    }
}
