namespace TecX.Common.Comparison
{
    using System;
    using System.Collections.Generic;

    public class EpsilonComparer : IComparer<double>
    {
        private static class Constants
        {
            /// <summary>0.0000000001</summary>
            public const double Epsilon = 0.00000000000001;
        }

        public static int Compare(double x, double y)
        {
            if (Math.Abs(x - y) <= x * Constants.Epsilon)
            {
                return 0;
            }

            return x < y ? -1 : 1;
        }

        public static bool AreEqual(double x, double y)
        {
            return Compare(x, y) == 0;
        }

        public static bool IsGreaterThan(double x, double y)
        {
            return Compare(x, y) > 0;
        }

        public static bool IsGreaterThanOrEqual(double x, double y)
        {
            return Compare(x, y) >= 0;
        }

        public static bool IsLessThan(double x, double y)
        {
            return Compare(x, y) < 0;
        }

        public static bool IsLessThanOrEqual(double x, double y)
        {
            return Compare(x, y) <= 0;
        }

        public static bool IsAlmostZero(double x)
        {
            return AreEqual(x, 0.0);
        }
        
        int IComparer<double>.Compare(double x, double y)
        {
            return Compare(x, y);
        }
    }
}