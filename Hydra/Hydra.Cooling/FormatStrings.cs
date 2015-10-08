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
            /// Supported by: All types derived from <see cref="Hydra.Cooling.Temperature">Hydra.Cooling.Temperature</see>.
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
            /// Supported by: All types derived from <see cref="Hydra.Cooling.Temperature">Hydra.Cooling.Temperature</see>.
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
            /// Supported by: All types derived from <see cref="Hydra.Cooling.Temperature">Hydra.Cooling.Temperature</see>.
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
            /// Supported by: All types derived from <see cref="Hydra.Cooling.Temperature">Hydra.Cooling.Temperature</see>.
            /// </para>
            /// <para>
            /// Precision specifier: Ignored.
            /// </para>
            /// </summary>
            public const string RoundTrip = "R";
        }

        public static class PhoneNumbers
        {
            /// <summary>
            /// <para>
            /// Result: Plain numeric value of any part of the <see cref="Hydra.Cooling.Alerts.PhoneNumber"/>
            /// </para>
            /// <para>
            /// Supported by: <see cref="Hydra.Cooling.Alerts.PhoneNumber">Hydra.Cooling.Alerts.PhoneNumber</see>, 
            /// <see cref="Hydra.Cooling.Alerts.CountryCode">Hydra.Cooling.Alerts.CountryCode</see>,
            /// <see cref="Hydra.Cooling.Alerts.AreaCode">Hydra.Cooling.Alerts.AreaCode</see>, 
            /// <see cref="Hydra.Cooling.Alerts.DialNumber">Hydra.Cooling.Alerts.DialNumber</see>, 
            /// <see cref="Hydra.Cooling.Alerts.PhoneExtension">Hydra.Cooling.Alerts.PhoneExtension</see>.
            /// </para>
            /// <para>
            /// Precision specifier: Ignored.
            /// </para>
            /// </summary>
            public const string Number = "N";

            /// <summary>
            /// <para>
            /// Result: Default representation of any part of the <see cref="Hydra.Cooling.Alerts.PhoneNumber"/>
            /// </para>
            /// <para>
            /// Supported by: <see cref="Hydra.Cooling.Alerts.PhoneNumber">Hydra.Cooling.Alerts.PhoneNumber</see>, 
            /// <see cref="Hydra.Cooling.Alerts.CountryCode">Hydra.Cooling.Alerts.CountryCode</see>,
            /// <see cref="Hydra.Cooling.Alerts.AreaCode">Hydra.Cooling.Alerts.AreaCode</see>, 
            /// <see cref="Hydra.Cooling.Alerts.DialNumber">Hydra.Cooling.Alerts.DialNumber</see>, 
            /// <see cref="Hydra.Cooling.Alerts.PhoneExtension">Hydra.Cooling.Alerts.PhoneExtension</see>.
            /// </para>
            /// <para>
            /// Precision specifier: Ignored.
            /// </para>
            /// </summary>
            public const string General = "G";
        }
    }
}
