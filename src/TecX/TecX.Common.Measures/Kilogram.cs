using System;
using System.Globalization;

namespace TecX.Common.Measures
{
    public struct Kilogram : IFormattable
    {
        private const double OrderOfThousand = 1000.0;

        private readonly double _value;

        public Kilogram(double value)
        {
            this._value = value;
        }

        #region Cast

        public static implicit operator Kilogram(double weight)
        {
            return new Kilogram(weight);
        }

        public static explicit operator double(Kilogram weight)
        {
            return weight._value;
        }

        #endregion Cast

        #region Add

        public static Kilogram operator +(Kilogram weight1, Ton weight2)
        {
            return new Kilogram(weight1._value + weight2.ToKilogram()._value);
        }

        public static Kilogram operator +(Kilogram weight1, Kilogram weight2)
        {
            return new Kilogram(weight1._value + weight2._value);
        }

        public static Kilogram operator +(Kilogram weight1, Gram weight2)
        {
            return new Kilogram(weight1._value + weight2.ToKilogram()._value);
        }

        public static Kilogram operator +(Kilogram weight1, Milligram weight2)
        {
            return new Kilogram(weight1._value + weight2.ToKilogram()._value);
        }

        public static Kilogram operator +(Kilogram weight1, double weight2)
        {
            return new Kilogram(weight1._value + weight2);
        }

        #endregion Add

        #region Subtract

        public static Kilogram operator -(Kilogram weight1, Ton weight2)
        {
            return new Kilogram(weight1._value - weight2.ToKilogram()._value);
        }

        public static Kilogram operator -(Kilogram weight1, Kilogram weight2)
        {
            return new Kilogram(weight1._value - weight2._value);
        }

        public static Kilogram operator -(Kilogram weight1, Gram weight2)
        {
            return new Kilogram(weight1._value - weight2.ToKilogram()._value);
        }

        public static Kilogram operator -(Kilogram weight1, Milligram weight2)
        {
            return new Kilogram(weight1._value - weight2.ToKilogram()._value);
        }

        public static Kilogram operator -(Kilogram weight1, double weight2)
        {
            return new Kilogram(weight1._value - weight2);
        }

        #endregion Subtract

        #region Conversion

        public Ton ToTon()
        {
            return new Ton(_value / OrderOfThousand);
        }

        public Gram ToGram()
        {
            return new Gram(_value * OrderOfThousand);
        }

        public Milligram ToMilligram()
        {
            return new Milligram(_value * OrderOfThousand * OrderOfThousand);
        }

        #endregion Conversion

        #region ToString

        public override string ToString()
        {
            return this.ToString("F3", CultureInfo.InvariantCulture);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return _value.ToString(format, formatProvider) + " kg";
        }

        #endregion ToString
    }
}
