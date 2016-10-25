namespace Cars.Measures
{
    public static class DistanceExtensions
    {
        public static Distance Kilometers(this double kilometers)
        {
            return Distance.FromKilometers(new decimal(kilometers));
        }

        public static Distance Kilometers(this decimal kilometers)
        {
            return Distance.FromKilometers(kilometers);
        }

        public static Distance Centimeters(this double centimeters)
        {
            return Distance.FromCentimeters(new decimal(centimeters));
        }

        public static Distance Centimeters(this decimal centimeters)
        {
            return Distance.FromCentimeters(centimeters);
        }
        
        public static Distance Millimeters(this double millimeters)
        {
            return Distance.FromMillimeters(new decimal(millimeters));
        }

        public static Distance Millimeters(this decimal millimeters)
        {
            return Distance.FromMillimeters(millimeters);
        }

        public static Distance Meters(this double meters)
        {
            return Distance.FromMeters(new decimal(meters));
        }

        public static Distance Meters(this decimal meters)
        {
            return Distance.FromMeters(meters);
        }
    }
}