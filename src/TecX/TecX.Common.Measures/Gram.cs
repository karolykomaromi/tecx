using System;
using System.Globalization;

namespace TecX.Common.Measures
{
    public struct Gram : IFormattable
    {
        private const double OrderOfThousand = 1000.0;

        private readonly double _value;

        public Gram(double value)
        {
            this._value = value;
        }

        #region Cast

        public static implicit operator Gram(double weight)
        {
            return new Gram(weight);
        }

        public static explicit operator double(Gram weight)
        {
            return weight._value;
        }

        #endregion Cast

        #region Add

        public static Gram operator +(Gram weight1, Ton weight2)
        {
            return new Gram(weight1._value + weight2.ToGram()._value);
        }

        public static Gram operator +(Gram weight1, Kilogram weight2)
        {
            return new Gram(weight1._value + weight2.ToGram()._value);
        }

        public static Gram operator +(Gram weight1, Gram weight2)
        {
            return new Gram(weight1._value + weight2._value);
        }

        public static Gram operator +(Gram weight1, Milligram weight2)
        {
            return new Gram(weight1._value + weight2.ToGram()._value);
        }

        public static Gram operator +(Gram weight1, double weight2)
        {
            return new Gram(weight1._value + weight2);
        }

        #endregion Add

        #region Subtract

        public static Gram operator -(Gram weight1, Ton weight2)
        {
            return new Gram(weight1._value - weight2.ToGram()._value);
        }

        public static Gram operator -(Gram weight1, Kilogram weight2)
        {
            return new Gram(weight1._value - weight2.ToGram()._value);
        }

        public static Gram operator -(Gram weight1, Gram weight2)
        {
            return new Gram(weight1._value - weight2._value);
        }

        public static Gram operator -(Gram weight1, Milligram weight2)
        {
            return new Gram(weight1._value - weight2.ToGram()._value);
        }

        public static Gram operator -(Gram weight1, double weight2)
        {
            return new Gram(weight1._value - weight2);
        }

        #endregion Subtract

        #region Conversion

        public Ton ToTon()
        {
            return new Ton(_value / (OrderOfThousand * OrderOfThousand));
        }

        public Kilogram ToKilogram()
        {
            return new Kilogram(_value / OrderOfThousand);
        }

        public Milligram ToMilligram()
        {
            return new Milligram(_value * OrderOfThousand);
        }

        #endregion Conversion

        #region ToString

        public override string ToString()
        {
            return this.ToString("F3", CultureInfo.InvariantCulture);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return _value.ToString(format, formatProvider) + " g";
        }

        #endregion ToString
    }
}