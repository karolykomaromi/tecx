namespace TecX.Common.Measures
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    [DebuggerDisplay("{value,{F2},{en-US}} m")]
    public struct Meter : IFormattable
    {
        private readonly double value;

        public Meter(double value)
        {
            this.value = value;
        }

        public static implicit operator Meter(double distance)
        {
            return new Meter(distance);
        }

        public static explicit operator double(Meter distance)
        {
            return distance.value;
        }

        public static Meter operator +(Meter distance1, Meter distance2)
        {
            return new Meter(distance1.value + distance2.value);
        }

        public static Meter operator +(Meter distance1, double distance2)
        {
            return new Meter(distance1.value + distance2);
        }

        public static Meter operator -(Meter distance1, Meter distance2)
        {
            return new Meter(distance1.value - distance2.value);
        }

        public static Meter operator -(Meter distance1, double distance2)
        {
            return new Meter(distance1.value - distance2);
        }

        public Kilometer ToKilometers()
        {
            return new Kilometer(this.value / 1000.0);
        }

        public override string ToString()
        {
            return this.ToString("F2", CultureInfo.InvariantCulture);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return this.value.ToString(format, formatProvider) + " m";
        }
    }
}
