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
    }
}
