namespace Hydra.Infrastructure.Test
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using Hydra.Infrastructure.I18n;
    using Xunit;
    using Xunit.Extensions;

    public class TemperatureTests
    {
        [Theory]
        [InlineData(2.5, "2,5°C")]
        [InlineData(-2.5, "-2,5°C")]
        [InlineData(0, "0,0°C")]
        public void Should_Correctly_Print_Celsius_Given_No_FormatString_And_German_Culture(double celsius, string expected)
        {
            Celsius c = celsius.DegreesCelsius();

            Assert.Equal(expected, c.ToString(null, Cultures.GermanGermany));
        }

        [Theory]
        [InlineData(2.5, "C2", "2,50°C")]
        [InlineData(-2.5, "C3", "-2,500°C")]
        [InlineData(0, "C0", "0°C")]
        public void Should_Correctly_Print_Celsius_Given_Specific_FormatString_And_German_Culture(double celsius, string format, string expected)
        {
            Celsius c = celsius.DegreesCelsius();

            Assert.Equal(expected, c.ToString(format, Cultures.GermanGermany));
        }

        [Theory]
        [InlineData(2.5, "2.5°C")]
        [InlineData(-2.5, "-2.5°C")]
        [InlineData(0, "0.0°C")]
        public void Should_Correctly_Print_Celsius_Given_No_FormatString_And_English_Culture(double celsius, string expected)
        {
            Celsius c = celsius.DegreesCelsius();

            Assert.Equal(expected, c.ToString(null, Cultures.EnglishUnitedStates));
        }

        [Theory]
        [InlineData(2.5, "C2", "2.50°C")]
        [InlineData(-2.5, "C3", "-2.500°C")]
        [InlineData(0, "C0", "0°C")]
        public void Should_Correctly_Print_Celsius_Given_Specific_FormatString_And_English_Culture(double celsius, string format, string expected)
        {
            Celsius c = celsius.DegreesCelsius();

            Assert.Equal(expected, c.ToString(format, Cultures.EnglishUnitedStates));
        }

        [Theory]
        [InlineData(2.5, "2,5°F")]
        [InlineData(-2.5, "-2,5°F")]
        [InlineData(0, "0,0°F")]
        public void Should_Correctly_Print_Fahrenheit_Given_No_FormatString_And_German_Culture(double fahrenheit, string expected)
        {
            Fahrenheit f = fahrenheit.DegreesFahrenheit();

            Assert.Equal(expected, f.ToString(null, Cultures.GermanGermany));
        }

        [Theory]
        [InlineData(2.5, "F2", "2,50°F")]
        [InlineData(-2.5, "F3", "-2,500°F")]
        [InlineData(0, "F0", "0°F")]
        public void Should_Correctly_Print_Fahrenheit_Given_Specific_FormatString_And_German_Culture(double fahrenheit, string format, string expected)
        {
            Fahrenheit f = fahrenheit.DegreesFahrenheit();

            Assert.Equal(expected, f.ToString(format, Cultures.GermanGermany));
        }

        [Theory]
        [InlineData(2.5, "2.5°F")]
        [InlineData(-2.5, "-2.5°F")]
        [InlineData(0, "0.0°F")]
        public void Should_Correctly_Print_Fahrenheit_Given_No_FormatString_And_English_Culture(double fahrenheit, string expected)
        {
            Fahrenheit f = fahrenheit.DegreesFahrenheit();

            Assert.Equal(expected, f.ToString(null, Cultures.EnglishUnitedStates));
        }

        [Theory]
        [InlineData(2.5, "F2", "2.50°F")]
        [InlineData(-2.5, "F3", "-2.500°F")]
        [InlineData(0, "F0", "0°F")]
        public void Should_Correctly_Print_Fahrenheit_Given_Specific_FormatString_And_English_Culture(double fahrenheit, string format, string expected)
        {
            Fahrenheit f = fahrenheit.DegreesFahrenheit();

            Assert.Equal(expected, f.ToString(format, Cultures.EnglishUnitedStates));
        }

        [Theory]
        [InlineData(2.5, "2,5K")]
        [InlineData(-2.5, "-2,5K")]
        [InlineData(0, "0,0K")]
        public void Should_Correctly_Print_Kelvin_Given_No_FormatString_And_German_Culture(double kelvin, string expected)
        {
            Kelvin f = kelvin.Kelvin();

            Assert.Equal(expected, f.ToString(null, Cultures.GermanGermany));
        }

        [Theory]
        [InlineData(2.5, "K2", "2,50K")]
        [InlineData(-2.5, "K3", "-2,500K")]
        [InlineData(0, "K0", "0K")]
        public void Should_Correctly_Print_Kelvin_Given_Specific_FormatString_And_German_Culture(double kelvin, string format, string expected)
        {
            Kelvin f = kelvin.Kelvin();

            Assert.Equal(expected, f.ToString(format, Cultures.GermanGermany));
        }

        [Theory]
        [InlineData(2.5, "2.5K")]
        [InlineData(-2.5, "-2.5K")]
        [InlineData(0, "0.0K")]
        public void Should_Correctly_Print_Kelvin_Given_No_FormatString_And_English_Culture(double kelvin, string expected)
        {
            Kelvin f = kelvin.Kelvin();

            Assert.Equal(expected, f.ToString(null, Cultures.EnglishUnitedStates));
        }

        [Theory]
        [InlineData(2.5, "K2", "2.50K")]
        [InlineData(-2.5, "K3", "-2.500K")]
        [InlineData(0, "K0", "0K")]
        public void Should_Correctly_Print_Kelvin_Given_Specific_FormatString_And_English_Culture(double kelvin, string format, string expected)
        {
            Kelvin f = kelvin.Kelvin();

            Assert.Equal(expected, f.ToString(format, Cultures.EnglishUnitedStates));
        }
    }

    public class CelsiusTests
    {
        [Theory]
        [InlineData(2.5, 3.7, 6.2)]
        [InlineData(0, 0, 0)]
        [InlineData(-10, 7.5, -2.5)]
        public void Should_Correctly_Add_Values(double x, double y, double z)
        {
            Celsius actual = x.DegreesCelsius() + y.DegreesCelsius();

            Celsius expected = z.DegreesCelsius();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2.5, 3.7, -1.2)]
        [InlineData(0, 0, 0)]
        [InlineData(-10, 7.5, -17.5)]
        public void Should_Correctly_Subtract_Values(double x, double y, double z)
        {
            Celsius actual = x.DegreesCelsius() - y.DegreesCelsius();

            Celsius expected = z.DegreesCelsius();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(3.5, 4.5)]
        [InlineData(4.49, 4.5)]
        [InlineData(-4.5, -4.49)]
        public void Should_Correctly_Identify_Lesser_Temperature(double lesser, double greater)
        {
            Celsius x = lesser.DegreesCelsius();
            Celsius y = greater.DegreesCelsius();

            Assert.True(x < y);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(3.5, 4.5)]
        [InlineData(-3.5, -3.5)]
        [InlineData(4.49, 4.5)]
        [InlineData(-4.5, -4.49)]
        [InlineData(-4.5, -4.5)]
        public void Should_Correctly_Identify_Lesser_Or_Equal_Temperature(double lesser, double greater)
        {
            Celsius x = lesser.DegreesCelsius();
            Celsius y = greater.DegreesCelsius();

            Assert.True(x <= y);
        }

        [Theory]
        [InlineData(3.5, 4.5)]
        [InlineData(4.49, 4.5)]
        [InlineData(-4.5, -4.49)]
        public void Should_Correctly_Identify_Greater_Temperature(double lesser, double greater)
        {
            Celsius x = lesser.DegreesCelsius();
            Celsius y = greater.DegreesCelsius();

            Assert.True(y > x);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(3.5, 4.5)]
        [InlineData(-3.5, -3.5)]
        [InlineData(4.49, 4.5)]
        [InlineData(-4.5, -4.49)]
        [InlineData(-4.5, -4.5)]
        public void Should_Correctly_Identify_Greater_Or_Equal_Temperature(double lesser, double greater)
        {
            Celsius x = lesser.DegreesCelsius();
            Celsius y = greater.DegreesCelsius();

            Assert.True(y >= x);
        }
    }

    public static class TemperatureExtensions
    {
        public static Celsius DegreesCelsius(this decimal celsius)
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

        public static Fahrenheit DegreesFahrenheit(this double fahrenheit)
        {
            return new Fahrenheit(new decimal(fahrenheit));
        }

        public static Kelvin Kelvin(this decimal kelvin)
        {
            return new Kelvin(kelvin);
        }

        public static Kelvin Kelvin(this double kelvin)
        {
            return new Kelvin(new decimal(kelvin));
        }
    }

    public abstract class Temperature : IFormattable
    {
        protected const int DefaultPrecision = 1;

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

        protected abstract string Unit { get; }

        public abstract Kelvin ToKelvin();

        public abstract Celsius ToCelsius();

        public abstract Fahrenheit ToFahrenheit();

        public override string ToString()
        {
            return this.ToString(this.Unit.TrimStart('°') + Temperature.DefaultPrecision, CultureInfo.CurrentCulture);
        }

        public string ToString(string format)
        {
            return this.ToString(format, CultureInfo.CurrentCulture);
        }

        public string ToString(IFormatProvider formatProvider)
        {
            string format = this.Unit.TrimStart('°') + Temperature.DefaultPrecision;

            return this.ToString(format, formatProvider);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            format = format ?? this.Unit.TrimStart('°') + Temperature.DefaultPrecision;
            formatProvider = formatProvider ?? CultureInfo.CurrentCulture;

            if (!Temperature.LegalFormatString.IsMatch(format))
            {
                throw new FormatException();
            }

            Match unit = Temperature.UnitIdentifier.Match(format);

            Temperature t;

            switch (unit.Value.ToUpperInvariant())
            {
                case "C":
                {
                    t = this.ToCelsius();
                    break;
                }
                case "K":
                {
                    t = this.ToKelvin();
                    break;
                }
                    case "F":
                {
                    t = this.ToFahrenheit();
                    break;
                }
                default:
                {
                    throw new ArgumentException("", "format");
                }
            }

            Match m = Temperature.Precision.Match(format);

            string precision = m.Success ? m.Value : Temperature.DefaultPrecision.ToString(CultureInfo.InvariantCulture);

            string decimalFormat = FormatStrings.Numeric.FixedPoint + precision;

            return t.Value.ToString(decimalFormat, formatProvider) + t.Unit;
        }
    }

    public class Celsius : Temperature, IComparable<Celsius>, IEquatable<Celsius>
    {
        public const string UnitSymbol = "°C";

        public static readonly Celsius AbsoluteZero = new Celsius(-273.15m);

        public Celsius(decimal celsius)
            : base(celsius)
        {
        }

        protected override string Unit
        {
            get { return Celsius.UnitSymbol; }
        }

        public static explicit operator Kelvin(Celsius celsius)
        {
            Contract.Requires(celsius != null);
            Contract.Ensures(Contract.Result<Kelvin>() != null);

            return celsius.ToKelvin();
        }

        public static explicit operator Fahrenheit(Celsius celsius)
        {
            Contract.Requires(celsius != null);
            Contract.Ensures(Contract.Result<Fahrenheit>() != null);

            return celsius.ToFahrenheit();
        }

        public static Celsius operator +(Celsius x, Celsius y)
        {
            Contract.Requires(x != null);
            Contract.Requires(y != null);
            Contract.Ensures(Contract.Result<Celsius>() != null);

            return new Celsius(x.Value + y.Value);
        }

        public static Celsius operator -(Celsius x, Celsius y)
        {
            Contract.Requires(x != null);
            Contract.Requires(y != null);
            Contract.Ensures(Contract.Result<Celsius>() != null);

            return new Celsius(x.Value - y.Value);
        }

        public static bool operator <(Celsius x, Celsius y)
        {
            Contract.Requires(x != null);
            Contract.Requires(y != null);

            return x.Value < y.Value;
        }

        public static bool operator >(Celsius x, Celsius y)
        {
            Contract.Requires(x != null);
            Contract.Requires(y != null);

            return x.Value > y.Value;
        }

        public static bool operator <=(Celsius x, Celsius y)
        {
            Contract.Requires(x != null);
            Contract.Requires(y != null);

            return x.Value <= y.Value;
        }

        public static bool operator >=(Celsius x, Celsius y)
        {
            Contract.Requires(x != null);
            Contract.Requires(y != null);

            return x.Value >= y.Value;
        }

        public static bool operator ==(Celsius x, Celsius y)
        {
            if (object.ReferenceEquals(x, null) && object.ReferenceEquals(y, null))
            {
                return true;
            }

            if (object.ReferenceEquals(x, null))
            {
                return false;
            }

            if (object.ReferenceEquals(y, null))
            {
                return false;
            }

            return x.Value.Equals(y.Value);
        }

        public static bool operator !=(Celsius x, Celsius y)
        {
            return !(x == y);
        }

        public static Celsius operator /(Celsius celsius, decimal factor)
        {
            Contract.Requires(celsius != null);
            Contract.Requires(factor != 0m);
            Contract.Ensures(Contract.Result<Celsius>() != null);

            return new Celsius(celsius.Value / factor);
        }

        public static Celsius operator *(Celsius celsius, decimal factor)
        {
            Contract.Requires(celsius != null);
            Contract.Ensures(Contract.Result<Celsius>() != null);

            return new Celsius(celsius.Value * factor);
        }

        public static Celsius operator *(decimal factor, Celsius celsius)
        {
            Contract.Requires(celsius != null);
            Contract.Ensures(Contract.Result<Celsius>() != null);

            return celsius * factor;
        }

        public override Kelvin ToKelvin()
        {
            return new Kelvin(this.Value + Kelvin.OffsetKelvinCelsius);
        }

        public override Celsius ToCelsius()
        {
            return this;
        }

        public override Fahrenheit ToFahrenheit()
        {
            return new Fahrenheit(this.Value - 32m * 5m / 9m);
        }

        public int CompareTo(Celsius other)
        {
            return this.Value.CompareTo((decimal)other.Value);
        }

        public bool Equals(Celsius other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            Celsius other = obj as Celsius;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }
    }

    public class Kelvin : Temperature, IComparable<Kelvin>, IEquatable<Kelvin>
    {
        public const string UnitSymbol = "K";

        public static readonly Kelvin AbsoluteZero = new Kelvin(0m);

        internal const decimal OffsetKelvinCelsius = 273.15m;

        public Kelvin(decimal kelvin)
            : base(kelvin)
        {
        }

        protected override string Unit
        {
            get { return Kelvin.UnitSymbol; }
        }

        public static explicit operator Celsius(Kelvin kelvin)
        {
            Contract.Requires(kelvin != null);
            Contract.Ensures(Contract.Result<Celsius>() != null);

            return kelvin.ToCelsius();
        }

        public static explicit operator Fahrenheit(Kelvin kelvin)
        {
            Contract.Requires(kelvin != null);
            Contract.Ensures(Contract.Result<Fahrenheit>() != null);

            return kelvin.ToFahrenheit();
        }

        public static Kelvin operator +(Kelvin x, Kelvin y)
        {
            Contract.Requires(x != null);
            Contract.Requires(y != null);
            Contract.Ensures(Contract.Result<Kelvin>() != null);

            return new Kelvin(x.Value + y.Value);
        }

        public static Kelvin operator -(Kelvin x, Kelvin y)
        {
            Contract.Requires(x != null);
            Contract.Requires(y != null);
            Contract.Ensures(Contract.Result<Kelvin>() != null);

            return new Kelvin(x.Value - y.Value);
        }
        
        public static bool operator <(Kelvin x, Kelvin y)
        {
            Contract.Requires(x != null);
            Contract.Requires(y != null);

            return x.Value < y.Value;
        }

        public static bool operator >(Kelvin x, Kelvin y)
        {
            Contract.Requires(x != null);
            Contract.Requires(y != null);

            return x.Value > y.Value;
        }

        public static bool operator <=(Kelvin x, Kelvin y)
        {
            Contract.Requires(x != null);
            Contract.Requires(y != null);

            return x.Value <= y.Value;
        }

        public static bool operator >=(Kelvin x, Kelvin y)
        {
            Contract.Requires(x != null);
            Contract.Requires(y != null);

            return x.Value >= y.Value;
        }

        public static bool operator ==(Kelvin x, Kelvin y)
        {
            Contract.Requires(x != null);
            Contract.Requires(y != null);

            return x.Value == y.Value;
        }

        public static bool operator !=(Kelvin x, Kelvin y)
        {
            Contract.Requires(x != null);
            Contract.Requires(y != null);

            return x.Value != y.Value;
        }

        public static Kelvin operator /(Kelvin kelvin, decimal factor)
        {
            Contract.Requires(kelvin != null);
            Contract.Requires(factor != 0m);
            Contract.Ensures(Contract.Result<Kelvin>() != null);

            return new Kelvin(kelvin.Value / factor);
        }

        public static Kelvin operator *(Kelvin kelvin, decimal factor)
        {
            Contract.Requires(kelvin != null);
            Contract.Ensures(Contract.Result<Kelvin>() != null);

            return new Kelvin(kelvin.Value * factor);
        }

        public static Kelvin operator *(decimal factor, Kelvin kelvin)
        {
            Contract.Requires(kelvin != null);
            Contract.Ensures(Contract.Result<Kelvin>() != null);

            return kelvin * factor;
        }

        public override Kelvin ToKelvin()
        {
            return this;
        }

        public override Celsius ToCelsius()
        {
            return new Celsius(this.Value - OffsetKelvinCelsius);
        }

        public override Fahrenheit ToFahrenheit()
        {
            return new Fahrenheit(this.Value * 1.8m - 459.67m);
        }

        public int CompareTo(Kelvin other)
        {
            return this.Value.CompareTo(other.Value);
        }

        public bool Equals(Kelvin other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            Kelvin other = obj as Kelvin;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public override string ToString()
        {
            return this.Value.ToString("F" + DefaultPrecision, CultureInfo.CurrentCulture) + this.Unit;
        }
    }

    public class Fahrenheit : Temperature, IComparable<Fahrenheit>, IEquatable<Fahrenheit>
    {
        public const string UnitSymbol = "°F";

        public static readonly Fahrenheit AbsoluteZero = new Fahrenheit(-459.67m);

        public Fahrenheit(decimal fahrenheit)
            : base(fahrenheit)
        {
        }

        protected override string Unit
        {
            get { return Fahrenheit.UnitSymbol; }
        }

        public static explicit operator Kelvin(Fahrenheit fahrenheit)
        {
            Contract.Requires(fahrenheit != null);
            Contract.Ensures(Contract.Result<Kelvin>() != null);

            return fahrenheit.ToKelvin();
        }

        public static explicit operator Celsius(Fahrenheit fahrenheit)
        {
            Contract.Requires(fahrenheit != null);
            Contract.Ensures(Contract.Result<Celsius>() != null);

            return fahrenheit.ToCelsius();
        }

        public static Fahrenheit operator +(Fahrenheit x, Fahrenheit y)
        {
            Contract.Requires(x != null);
            Contract.Requires(y != null);
            Contract.Ensures(Contract.Result<Fahrenheit>() != null);

            return new Fahrenheit(x.Value + y.Value);
        }

        public static Fahrenheit operator -(Fahrenheit x, Fahrenheit y)
        {
            Contract.Requires(x != null);
            Contract.Requires(y != null);
            Contract.Ensures(Contract.Result<Fahrenheit>() != null);

            return new Fahrenheit(x.Value - y.Value);
        }

        public static bool operator <(Fahrenheit x, Fahrenheit y)
        {
            Contract.Requires(x != null);
            Contract.Requires(y != null);

            return x.Value < y.Value;
        }

        public static bool operator >(Fahrenheit x, Fahrenheit y)
        {
            Contract.Requires(x != null);
            Contract.Requires(y != null);

            return x.Value > y.Value;
        }

        public static bool operator <=(Fahrenheit x, Fahrenheit y)
        {
            Contract.Requires(x != null);
            Contract.Requires(y != null);

            return x.Value <= y.Value;
        }

        public static bool operator >=(Fahrenheit x, Fahrenheit y)
        {
            Contract.Requires(x != null);
            Contract.Requires(y != null);

            return x.Value >= y.Value;
        }

        public static bool operator ==(Fahrenheit x, Fahrenheit y)
        {
            Contract.Requires(x != null);
            Contract.Requires(y != null);

            return x.Value == y.Value;
        }

        public static bool operator !=(Fahrenheit x, Fahrenheit y)
        {
            Contract.Requires(x != null);
            Contract.Requires(y != null);

            return x.Value != y.Value;
        }

        public static Fahrenheit operator /(Fahrenheit fahrenheit, decimal factor)
        {
            Contract.Requires(fahrenheit != null);
            Contract.Requires(factor != 0m);
            Contract.Ensures(Contract.Result<Fahrenheit>() != null);

            return new Fahrenheit(fahrenheit.Value / factor);
        }

        public static Fahrenheit operator *(Fahrenheit fahrenheit, decimal factor)
        {
            Contract.Requires(fahrenheit != null);
            Contract.Ensures(Contract.Result<Fahrenheit>() != null);

            return new Fahrenheit(fahrenheit.Value * factor);
        }

        public static Fahrenheit operator *(decimal factor, Fahrenheit fahrenheit)
        {
            Contract.Requires(fahrenheit != null);
            Contract.Ensures(Contract.Result<Fahrenheit>() != null);

            return fahrenheit * factor;
        }

        public override Kelvin ToKelvin()
        {
            return new Kelvin(this.Value * 1.8m - 459.67m);
        }

        public override Celsius ToCelsius()
        {
            return new Celsius(this.Value * 1.8m + 32m);
        }

        public override Fahrenheit ToFahrenheit()
        {
            return this;
        }

        public int CompareTo(Fahrenheit other)
        {
            return this.Value.CompareTo(other.Value);
        }

        public bool Equals(Fahrenheit other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            Fahrenheit other = obj as Fahrenheit;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }
    }
}
