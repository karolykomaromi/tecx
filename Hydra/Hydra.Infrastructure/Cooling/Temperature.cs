namespace Hydra.Infrastructure.Cooling
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;

    public abstract class Temperature : IFormattable
    {
        private const string DefaultPrecision = "1";

        private static readonly Regex LegalFormatString = new Regex("^[cfk][0-9]*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex UnitIdentifier = new Regex("^[cfk]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

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
                case Celsius.FormatString:
                {
                    temperature = this.ToCelsius();
                    break;
                }

                case Kelvin.FormatString:
                {
                    temperature = this.ToKelvin();
                    break;
                }

                case Fahrenheit.FormatString:
                {
                    temperature = this.ToFahrenheit();
                    break;
                }

                default:
                {
                    throw new FormatException();
                }
            }

            match = Temperature.Precision.Match(format);

            string precision = match.Success ? match.Value : Temperature.DefaultPrecision;

            string decimalFormat = FormatStrings.Numeric.FixedPoint + precision;

            return temperature.Value.ToString(decimalFormat, formatProvider) + temperature.Symbol;
        }
    }
}