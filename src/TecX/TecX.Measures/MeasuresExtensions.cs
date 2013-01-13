namespace TecX.Measures
{
    public static class MeasuresExtensions
    {
        public static KilometersPerHour Kmh(this double speed)
        {
            return new KilometersPerHour(speed);
        }

        public static KilometersPerHour Kmh(this int speed)
        {
            return new KilometersPerHour(speed);
        }

        public static Kilometer Kilometers(this double distance)
        {
            return new Kilometer(distance);
        }

        public static Kilometer Kilometers(this int distance)
        {
            return new Kilometer(distance);
        }

        public static Meter Meters(this double distance)
        {
            return new Meter(distance);
        }

        public static Meter Meters(this int distance)
        {
            return new Meter(distance);
        }

        public static Weight Tons(this double weight)
        {
            return Weight.FromTons(weight);
        }

        public static Weight Tons(this int weight)
        {
            return Weight.FromTons(weight);
        }

        public static Weight Kilograms(this double weight)
        {
            return Weight.FromKilograms(weight);
        }

        public static Weight Kilograms(this int weight)
        {
            return Weight.FromKilograms(weight);
        }

        public static Weight Grams(this double weight)
        {
            return Weight.FromGrams(weight);
        }

        public static Weight Grams(this int weight)
        {
            return Weight.FromGrams(weight);
        }

        public static Weight Milligrams(this double weight)
        {
            return Weight.FromMilligrams(weight);
        }

        public static Weight Milligrams(this int weight)
        {
            return Weight.FromMilligrams(weight);
        }
    }
}
