namespace Hydra.Cooling
{
    public static class FormatStrings
    {
        public static class Temperatures
        {
            /// <summary>
            /// <para>
            /// Result: A temperature value in °C (degrees Celsius).
            /// </para>
            /// <para>
            /// Supported by: All types derived from <see cref="Hydra.Cooling.Temperature"/>.
            /// </para>
            /// <para>
            /// Precision specifier: Number of decimal digits.
            /// </para>
            /// <para>
            /// Default precision specifier: Defined by <see cref="Hydra.Cooling.Temperature.DefaultPrecision">Hydra.Infrastructure.Cooling.Temperature.DefaultPrecision</see>.
            /// </para>
            /// </summary>
            public const string Celsius = "C";

            /// <summary>
            /// <para>
            /// Result: A temperature value in K (Kelvin).
            /// </para>
            /// <para>
            /// Supported by: All types derived from <see cref="Hydra.Cooling.Temperature"/>.
            /// </para>
            /// <para>
            /// Precision specifier: Number of decimal digits.
            /// </para>
            /// <para>
            /// Default precision specifier: Defined by <see cref="Hydra.Cooling.Temperature.DefaultPrecision">Hydra.Infrastructure.Cooling.Temperature.DefaultPrecision</see>.
            /// </para>
            /// </summary>
            public const string Kelvin = "K";

            /// <summary>
            /// <para>
            /// Result: A temperature value in °F (degrees Fahrenheit).
            /// </para>
            /// <para>
            /// Supported by: All types derived from <see cref="Hydra.Cooling.Temperature"/>.
            /// </para>
            /// <para>
            /// Precision specifier: Number of decimal digits.
            /// </para>
            /// <para>
            /// Default precision specifier: Defined by <see cref="Hydra.Cooling.Temperature.DefaultPrecision">Hydra.Infrastructure.Cooling.Temperature.DefaultPrecision</see>.
            /// </para>
            /// </summary>
            public const string Fahrenheit = "F";

            /// <summary>
            /// <para>
            /// Result: A string that can round-trip to an identical number.
            /// </para>
            /// <para>
            /// Supported by: All types derived from <see cref="Hydra.Cooling.Temperature"/>.
            /// </para>
            /// <para>
            /// Precision specifier: Ignored.
            /// </para>
            /// </summary>
            public const string RoundTrip = "R";
        }
    }
}
