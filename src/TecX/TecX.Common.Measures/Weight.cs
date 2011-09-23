using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TecX.Common.Measures
{
    [Serializable, StructLayout(LayoutKind.Sequential), ComVisible(true)]
    public struct Weight : IComparable<Weight>, IComparable, IEquatable<Weight>
    {
        #region Constants

        private static class Constants
        {
            public const long TicksPerMilligram = 0x2710L;
            public const double MilligramsPerTick = 1E-04;

            public const long TicksPerGram = 0x989680L;
            public const double GramsPerTick = 1E-07;

            public const long TicksPerKilogram = 0x2540BE400L;
            public const double KilogramsPerTick = 1E-10;

            public const long TicksPerTon = 0x9184E72A000L;
            public const double TonsPerTick = 1E-13;

            public const int MillisPerGram = 0x3E8;
            public const int MillisPerKilo = 0xF4240;
            public const int MillisPerTon = 0x3B9ACA00;

            public const long MaxMilligrams = 0x346dc5d638865L;
            public const long MinMilligrams = -922337203685477L;
        }

        public static readonly Weight MaxWeight;
        public static readonly Weight MinWeight;
        public static readonly Weight Zero;

        #endregion

        #region Fields

        private readonly long _ticks;

        #endregion Fields

        #region Properties

        public long Ticks
        {
            get { return _ticks; }
        }

        public int Milligrams
        {
            get { return (int)(_ticks / Constants.TicksPerMilligram); }
        }

        public int Grams
        {
            get { return (int)(_ticks / Constants.TicksPerGram); }
        }

        public int Kilograms
        {
            get { return (int)(_ticks / Constants.TicksPerKilogram); }
        }

        public int Tons
        {
            get { return (int)(_ticks / Constants.TicksPerTon); }
        }

        public double TotalMilligrams
        {
            get { return _ticks * Constants.MilligramsPerTick; }
        }

        public double TotalGrams
        {
            get { return _ticks * Constants.GramsPerTick; }
        }

        public double TotalKilograms
        {
            get { return _ticks * Constants.KilogramsPerTick; }
        }

        public double TotalTons
        {
            get { return _ticks * Constants.TonsPerTick; }
        }

        #endregion Properties

        #region c'tor

        static Weight()
        {
            MaxWeight = new Weight(long.MaxValue);
            MinWeight = new Weight(long.MinValue);
            Zero = new Weight(0L);
        }

        public Weight(long ticks)
        {
            _ticks = ticks;
        }

        public Weight(int kilograms, int grams)
            : this(0, kilograms, grams)
        {
        }

        public Weight(int tons, int kilograms, int grams)
            : this(tons, kilograms, grams, 0)
        {
        }

        public Weight(int tons, int kilograms, int grams, int milligrams)
        {
            long num = tons * Constants.MillisPerTon +
                       kilograms * Constants.MillisPerKilo +
                       grams * Constants.MillisPerGram +
                       milligrams;

            if ((num > Constants.MaxMilligrams) || (num < Constants.MinMilligrams))
            {
                throw new ArgumentOutOfRangeException(null, "Weight too heavy.");
            }

            _ticks = num * Constants.TicksPerMilligram;
        }

        #endregion c'tor

        public static Weight FromMilligrams(double value)
        {
            return Interval(value, 1);
        }

        public static Weight FromGrams(double value)
        {
            return Interval(value, Constants.MillisPerGram);
        }

        public static Weight FromKilograms(double value)
        {
            return Interval(value, Constants.MillisPerKilo);
        }

        public static Weight FromTons(double value)
        {
            return Interval(value, Constants.MillisPerTon);
        }

        private static Weight Interval(double value, int scale)
        {
            if (double.IsNaN(value))
            {
                throw new ArgumentException("Argument cannot be NaN.", "value");
            }

            double num = value * scale;
            double num2 = num + ((value >= 0.0) ? 0.5 : -0.5);

            if ((num2 > 922337203685477) || (num2 < -922337203685477))
            {
                throw new OverflowException("Weight too heavy");
            }

            return new Weight(((long)num2) * Constants.TicksPerMilligram);
        }

        public int CompareTo(Weight other)
        {
            return _ticks.CompareTo(other._ticks);
        }

        int IComparable.CompareTo(object obj)
        {
            Guard.AssertNotNull(obj, "obj");

            if (obj is Weight)
            {
                return CompareTo((Weight)obj);
            }

            throw new ArgumentException("Argument is not of Type Weight", "obj");
        }

        public bool Equals(Weight other)
        {
            return _ticks.Equals(other._ticks);
        }

        public static Weight FromTicks(long ticks)
        {
            return new Weight(ticks);
        }
    }
}
