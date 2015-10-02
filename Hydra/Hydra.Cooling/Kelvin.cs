namespace Hydra.Cooling
{
    using System;
    using System.Diagnostics.Contracts;

    public class Kelvin : Temperature, IComparable<Kelvin>, IEquatable<Kelvin>
    {
        public const string UnitSymbol = "K";

        public static readonly Kelvin AbsoluteZero = new Kelvin(0m);

        public static new readonly Kelvin Invalid = new Kelvin(decimal.MinValue);

        internal const decimal OffsetKelvinCelsius = 273.15m;

        public Kelvin(decimal kelvin)
            : base(kelvin)
        {
        }

        protected override string Format
        {
            get { return FormatStrings.Temperatures.Kelvin; }
        }

        protected override string Symbol
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

        public static bool operator >(Kelvin x, Kelvin y)
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

        public static bool operator <=(Kelvin x, Kelvin y)
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

        public static bool operator >=(Kelvin x, Kelvin y)
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

        public static bool operator ==(Kelvin x, Kelvin y)
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

        public static bool operator !=(Kelvin x, Kelvin y)
        {
            return !(x == y);
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
            return new Fahrenheit((this.Value * 1.8m) - 459.67m);
        }

        public int CompareTo(Kelvin other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return 1;
            }

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
    }
}