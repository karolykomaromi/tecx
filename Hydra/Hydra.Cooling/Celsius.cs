namespace Hydra.Cooling
{
    using System;
    using System.Diagnostics.Contracts;

    public class Celsius : Temperature, IComparable<Celsius>, IEquatable<Celsius>
    {
        public const string UnitSymbol = "°C";

        public static readonly Celsius AbsoluteZero = new Celsius(-273.15m);

        public static readonly Celsius WaterBoiling = new Celsius(100.00m);

        public static readonly Celsius WaterFreezing = new Celsius(0m);

        public static new readonly Celsius Invalid = new Celsius(decimal.MinValue);

        public Celsius(decimal celsius)
            : base(celsius)
        {
        }

        protected override string Symbol
        {
            get { return Celsius.UnitSymbol; }
        }

        protected override string Format
        {
            get { return FormatStrings.Temperatures.Celsius; }
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
            if (object.ReferenceEquals(x, y))
            {
                return false;
            }

            if (object.ReferenceEquals(x, null))
            {
                return true;
            }

            if (object.ReferenceEquals(y, null))
            {
                return false;
            }

            return x.Value < y.Value;
        }

        public static bool operator >(Celsius x, Celsius y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return false;
            }

            if (object.ReferenceEquals(x, null))
            {
                return false;
            }

            if (object.ReferenceEquals(y, null))
            {
                return true;
            }

            return x.Value > y.Value;
        }

        public static bool operator <=(Celsius x, Celsius y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return true;
            }

            if (object.ReferenceEquals(x, null))
            {
                return true;
            }

            if (object.ReferenceEquals(y, null))
            {
                return false;
            }

            return x.Value <= y.Value;
        }

        public static bool operator >=(Celsius x, Celsius y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return true;
            }

            if (object.ReferenceEquals(x, null))
            {
                return false;
            }

            if (object.ReferenceEquals(y, null))
            {
                return true;
            }

            return x.Value >= y.Value;
        }

        public static bool operator ==(Celsius x, Celsius y)
        {
            if (object.ReferenceEquals(x, y))
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
            return new Fahrenheit(this.Value - (32m * 5m / 9m));
        }

        public int CompareTo(Celsius other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return 1;
            }

            return this.Value.CompareTo(other.Value);
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
}