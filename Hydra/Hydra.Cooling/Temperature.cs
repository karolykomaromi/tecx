namespace Hydra.Cooling
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;

    public abstract class Temperature : IFormattable
    {
        public static readonly Temperature Invalid = new InvalidTemperature();

        private const string DefaultPrecision = "1";

        private static readonly Regex LegalFormatString = new Regex("^[rcfk][0-9]*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex UnitIdentifier = new Regex("^[rcfk]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex Precision = new Regex("[0-9]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly decimal value;

        protected Temperature(decimal value)
        {
            this.value = value;
        }

        public decimal Value
        {
            get { return this.value; }
        }

        protected abstract string Symbol { get; }

        protected abstract string Format { get; }

        public static bool TryParse(string s, out Temperature temperature)
        {
            return Temperature.TryParse(s, NumberStyles.Number, CultureInfo.CurrentCulture, out temperature);
        }

        public static bool TryParse(string s, NumberStyles style, IFormatProvider formatProvider, out Temperature temperature)
        {
            if (string.IsNullOrEmpty(s))
            {
                temperature = Temperature.Invalid;
                return false;
            }

            if (s.EndsWith(Celsius.UnitSymbol, StringComparison.Ordinal))
            {
                decimal t;
                if (!decimal.TryParse(s.Substring(0, s.Length - 2), style, formatProvider, out t))
                {
                    temperature = Temperature.Invalid;
                    return false;
                }

                temperature = new Celsius(t);
                return true;
            }

            if (s.EndsWith(Fahrenheit.UnitSymbol, StringComparison.Ordinal))
            {
                decimal t;
                if (!decimal.TryParse(s.Substring(0, s.Length - 2), style, formatProvider, out t))
                {
                    temperature = Temperature.Invalid;
                    return false;
                }

                temperature = new Fahrenheit(t);
                return true;
            }

            if (s.EndsWith(Kelvin.UnitSymbol, StringComparison.Ordinal))
            {
                decimal t;
                if (!decimal.TryParse(s.Substring(0, s.Length - 1), style, formatProvider, out t))
                {
                    temperature = Temperature.Invalid;
                    return false;
                }

                temperature = new Kelvin(t);
                return true;
            }

            temperature = Temperature.Invalid;
            return false;
        }

        public abstract Kelvin ToKelvin();

        public abstract Celsius ToCelsius();

        public abstract Fahrenheit ToFahrenheit();

        public override string ToString()
        {
            return this.ToString(this.Format + Temperature.DefaultPrecision, CultureInfo.CurrentCulture);
        }

        public string ToString(string format)
        {
            return this.ToString(format, CultureInfo.CurrentCulture);
        }

        public string ToString(IFormatProvider formatProvider)
        {
            string format = this.Format + Temperature.DefaultPrecision;

            return this.ToString(format, formatProvider);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            format = format ?? this.Format + Temperature.DefaultPrecision;
            formatProvider = formatProvider ?? CultureInfo.CurrentCulture;

            if (!Temperature.LegalFormatString.IsMatch(format))
            {
                throw new FormatException();
            }

            Match match = Temperature.UnitIdentifier.Match(format);

            Temperature temperature;

            string unit = match.Value.ToUpperInvariant();

            switch (unit)
            {
                case FormatStrings.Temperatures.Celsius:
                {
                    temperature = this.ToCelsius();
                    break;
                }

                case FormatStrings.Temperatures.Kelvin:
                {
                    temperature = this.ToKelvin();
                    break;
                }

                case FormatStrings.Temperatures.Fahrenheit:
                {
                    temperature = this.ToFahrenheit();
                    break;
                }

                case FormatStrings.Temperatures.RoundTrip:
                {
                    return this.Value.ToString(CultureInfo.InvariantCulture) + this.Symbol;
                }

                default:
                {
                    throw new FormatException();
                }
            }

            match = Temperature.Precision.Match(format);

            string precision = match.Success ? match.Value : Temperature.DefaultPrecision;

            string decimalFormat = Infrastructure.FormatStrings.Numeric.FixedPoint + precision;

            return temperature.Value.ToString(decimalFormat, formatProvider) + temperature.Symbol;
        }

        private class InvalidTemperature : Temperature
        {
            public InvalidTemperature()
                : base(decimal.MinValue)
            {
            }

            protected override string Symbol
            {
                get { return string.Empty; }
            }

            protected override string Format
            {
                get { return string.Empty; }
            }

            public override Kelvin ToKelvin()
            {
                return Kelvin.Invalid;
            }

            public override Celsius ToCelsius()
            {
                return Celsius.Invalid;
            }

            public override Fahrenheit ToFahrenheit()
            {
                return Fahrenheit.Invalid;
            }
        }
    }
}