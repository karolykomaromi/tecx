namespace TecX.Common.Measures
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

        public static Ton Tons(this double weight)
        {
            return new Ton(weight);
        }

        public static Ton Tons(this int weight)
        {
            return new Ton(weight);
        }

        public static Kilogram Kilograms(this double weight)
        {
            return new Kilogram(weight);
        }

        public static Kilogram Kilograms(this int weight)
        {
            return new Kilogram(weight);
        }

        public static Gram Grams(this double weight)
        {
            return new Gram(weight);
        }

        public static Gram Grams(this int weight)
        {
            return new Gram(weight);
        }

        public static Milligram Milligrams(this double weight)
        {
            return new Milligram(weight);
        }

        public static Milligram Milligrams(this int weight)
        {
            return new Milligram(weight);
        }
    }
}
