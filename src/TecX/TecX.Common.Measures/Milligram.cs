namespace TecX.Common.Measures
{
    using System;
    using System.Globalization;

    public struct Milligram : IFormattable
    {
        private const double OrderOfThousand = 1000.0;

        private readonly double _value;

        public Milligram(double value)
        {
            this._value = value;
        }

        #region Cast

        public static implicit operator Milligram(double weight)
        {
            return new Milligram(weight);
        }

        public static explicit operator double(Milligram weight)
        {
            return weight._value;
        }

        #endregion Cast

        #region Add

        public static Milligram operator +(Milligram weight1, Ton weight2)
        {
            return new Milligram(weight1._value + weight2.ToMilligram()._value);
        }

        public static Milligram operator +(Milligram weight1, Kilogram weight2)
        {
            return new Milligram(weight1._value + weight2.ToMilligram()._value);
        }

        public static Milligram operator +(Milligram weight1, Gram weight2)
        {
            return new Milligram(weight1._value + weight2.ToMilligram()._value);
        }

        public static Milligram operator +(Milligram weight1, Milligram weight2)
        {
            return new Milligram(weight1._value + weight2._value);
        }

        public static Milligram operator +(Milligram weight1, double weight2)
        {
            return new Milligram(weight1._value + weight2);
        }

        #endregion Add

        #region Subtract

        public static Milligram operator -(Milligram weight1, Ton weight2)
        {
            return new Milligram(weight1._value - weight2.ToMilligram()._value);
        }

        public static Milligram operator -(Milligram weight1, Kilogram weight2)
        {
            return new Milligram(weight1._value - weight2.ToMilligram()._value);
        }

        public static Milligram operator -(Milligram weight1, Gram weight2)
        {
            return new Milligram(weight1._value - weight2.ToMilligram()._value);
        }

        public static Milligram operator -(Milligram weight1, Milligram weight2)
        {
            return new Milligram(weight1._value - weight2._value);
        }

        public static Milligram operator -(Milligram weight1, double weight2)
        {
            return new Milligram(weight1._value - weight2);
        }

        #endregion Subtract

        #region Conversion

        public Ton ToTon()
        {
            return new Ton(_value / (OrderOfThousand * OrderOfThousand * OrderOfThousand));
        }

        public Kilogram ToKilogram()
        {
            return new Kilogram(_value / (OrderOfThousand * OrderOfThousand));
        }

        public Gram ToGram()
        {
            return new Gram(_value / (OrderOfThousand));
        }

        #endregion Conversion

        #region ToString

        public override string ToString()
        {
            return this.ToString("F3", CultureInfo.InvariantCulture);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return _value.ToString(format, formatProvider) + " mg";
        }

        #endregion ToString
    }
}