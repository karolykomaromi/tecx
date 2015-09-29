namespace Hydra.Infrastructure.Cooling
{
    using System;
    using System.Diagnostics.Contracts;

    public class Fahrenheit : Temperature, IComparable<Fahrenheit>, IEquatable<Fahrenheit>
    {
        public const string UnitSymbol = "°F";

        public static readonly Fahrenheit AbsoluteZero = new Fahrenheit(-459.67m);
        
        internal const string FormatString = "F";

        public Fahrenheit(decimal fahrenheit)
            : base(fahrenheit)
        {
        }

        protected override string Format
        {
            get { return Fahrenheit.FormatString; }
        }

        protected override string Symbol
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

        public static bool operator >(Fahrenheit x, Fahrenheit y)
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

        public static bool operator <=(Fahrenheit x, Fahrenheit y)
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

        public static bool operator >=(Fahrenheit x, Fahrenheit y)
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

        public static bool operator ==(Fahrenheit x, Fahrenheit y)
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

        public static bool operator !=(Fahrenheit x, Fahrenheit y)
        {
            return !(x == y);
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
            return new Kelvin((this.Value * 1.8m) - 459.67m);
        }

        public override Celsius ToCelsius()
        {
            return new Celsius((this.Value * 1.8m) + 32m);
        }

        public override Fahrenheit ToFahrenheit()
        {
            return this;
        }

        public int CompareTo(Fahrenheit other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return 1;
            }

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