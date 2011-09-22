using System;
using System.Globalization;

namespace TecX.Common.Measures
{
    public struct Ton : IFormattable
    {
        private readonly double _value;

        private const double OrderOfThousand = 1000.0;

        public Ton(double value)
        {
            this._value = value;
        }

        #region Cast

        public static implicit operator Ton(double weight)
        {
            return new Ton(weight);
        }

        public static explicit operator double(Ton weight)
        {
            return weight._value;
        }

        #endregion Cast

        #region Add

        public static Ton operator +(Ton weight1, Ton weight2)
        {
            return new Ton(weight1._value + weight2._value);
        }

        public static Ton operator +(Ton weight1, Kilogram weight2)
        {
            return new Ton(weight1._value + weight2.ToTon()._value);
        }

        public static Ton operator +(Ton weight1, Gram weight2)
        {
            return new Ton(weight1._value + weight2.ToTon()._value);
        }

        public static Ton operator +(Ton weight1, Milligram weight2)
        {
            return new Ton(weight1._value + weight2.ToTon()._value);
        }

        public static Ton operator +(Ton weight1, double weight2)
        {
            return new Ton(weight1._value + weight2);
        }

        #endregion Add

        #region Subtract

        public static Ton operator -(Ton weight1, Ton weight2)
        {
            return new Ton(weight1._value - weight2._value);
        }

        public static Ton operator -(Ton weight1, Kilogram weight2)
        {
            return new Ton(weight1._value - weight2.ToTon()._value);
        }

        public static Ton operator -(Ton weight1, Gram weight2)
        {
            return new Ton(weight1._value - weight2.ToTon()._value);
        }

        public static Ton operator -(Ton weight1, Milligram weight2)
        {
            return new Ton(weight1._value - weight2.ToTon()._value);
        }

        public static Ton operator -(Ton weight1, double weight2)
        {
            return new Ton(weight1._value - weight2);
        }

        #endregion Subtract

        #region Conversion

        public Kilogram ToKilogram()
        {
            return new Kilogram(_value * OrderOfThousand);
        }

        public Gram ToGram()
        {
            return new Gram(_value * (OrderOfThousand * OrderOfThousand));
        }

        public Milligram ToMilligram()
        {
            return new Milligram(_value * (OrderOfThousand * OrderOfThousand * OrderOfThousand));
        }

        #endregion Conversion

        #region ToString

        public override string ToString()
        {
            return this.ToString("F3", CultureInfo.InvariantCulture);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return _value.ToString(format, formatProvider) + " t";
        }

        #endregion ToString
    }
}