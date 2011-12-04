namespace TecX.Common.Measures
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [ComVisible(true)]
    public struct Weight : IComparable<Weight>, IComparable, IEquatable<Weight>
    {
        #region Constants

        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder",
            Justification = "Reviewed. Suppression is OK here.")]
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

        #endregion Constants

        #region Fields

        private readonly long ticks;

        #endregion Fields

        #region c'tor

        static Weight()
        {
            MaxWeight = new Weight(long.MaxValue);
            MinWeight = new Weight(long.MinValue);
            Zero = new Weight(0L);
        }

        public Weight(long ticks)
        {
            this.ticks = ticks;
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
            long num = (tons * (long)Constants.MillisPerTon) + 
                (kilograms * (long)Constants.MillisPerKilo) + 
                (grams * (long)Constants.MillisPerGram) + 
                milligrams;

            if ((num > Constants.MaxMilligrams) || (num < Constants.MinMilligrams))
            {
                throw new ArgumentOutOfRangeException(null, "Weight too heavy.");
            }

            this.ticks = num * Constants.TicksPerMilligram;
        }

        #endregion c'tor

        #region Properties

        public long Ticks
        {
            get
            {
                return this.ticks;
            }
        }

        public int Milligrams
        {
            get
            {
                return (int)(this.ticks / Constants.TicksPerMilligram);
            }
        }

        public int Grams
        {
            get
            {
                return (int)(this.ticks / Constants.TicksPerGram);
            }
        }

        public int Kilograms
        {
            get
            {
                return (int)(this.ticks / Constants.TicksPerKilogram);
            }
        }

        public int Tons
        {
            get
            {
                return (int)(this.ticks / Constants.TicksPerTon);
            }
        }

        public double TotalMilligrams
        {
            get
            {
                return this.ticks * Constants.MilligramsPerTick;
            }
        }

        public double TotalGrams
        {
            get
            {
                return this.ticks * Constants.GramsPerTick;
            }
        }

        public double TotalKilograms
        {
            get
            {
                return this.ticks * Constants.KilogramsPerTick;
            }
        }

        public double TotalTons
        {
            get
            {
                return this.ticks * Constants.TonsPerTick;
            }
        }

        #endregion Properties

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

        public static Weight FromTicks(long ticks)
        {
            return new Weight(ticks);
        }

        public int CompareTo(Weight other)
        {
            return this.ticks.CompareTo(other.ticks);
        }

        public bool Equals(Weight other)
        {
            return this.ticks.Equals(other.ticks);
        }

        int IComparable.CompareTo(object obj)
        {
            Guard.AssertNotNull(obj, "obj");

            if (obj is Weight)
            {
                return this.CompareTo((Weight)obj);
            }

            throw new ArgumentException("Argument is not of Type Weight", "obj");
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
    }
}