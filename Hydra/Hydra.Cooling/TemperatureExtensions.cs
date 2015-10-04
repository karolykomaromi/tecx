namespace Hydra.Cooling
{
    public static class TemperatureExtensions
    {
        public static Celsius DegreesCelsius(this decimal celsius)
        {
            return new Celsius(celsius);
        }

        public static Celsius DegreesCelsius(this long celsius)
        {
            return new Celsius(celsius);
        }

        public static Celsius DegreesCelsius(this int celsius)
        {
            return new Celsius(celsius);
        }

        public static Celsius DegreesCelsius(this double celsius)
        {
            return new Celsius(new decimal(celsius));
        }

        public static Fahrenheit DegreesFahrenheit(this decimal fahrenheit)
        {
            return new Fahrenheit(fahrenheit);
        }

        public static Fahrenheit DegreesFahrenheit(this long fahrenheit)
        {
            return new Fahrenheit(fahrenheit);
        }

        public static Fahrenheit DegreesFahrenheit(this int fahrenheit)
        {
            return new Fahrenheit(fahrenheit);
        }

        public static Fahrenheit DegreesFahrenheit(this double fahrenheit)
        {
            return new Fahrenheit(new decimal(fahrenheit));
        }

        public static Kelvin Kelvin(this decimal kelvin)
        {
            return new Kelvin(kelvin);
        }

        public static Kelvin Kelvin(this long kelvin)
        {
            return new Kelvin(kelvin);
        }

        public static Kelvin Kelvin(this int kelvin)
        {
            return new Kelvin(kelvin);
        }

        public static Kelvin Kelvin(this double kelvin)
        {
            return new Kelvin(new decimal(kelvin));
        }
    }
}