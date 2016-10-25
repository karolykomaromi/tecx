namespace Cars.Measures
{
    using System;
    using System.Diagnostics.Contracts;

    public struct Torque : IEquatable<Torque>, IComparable<Torque>
    {
        public static readonly Torque Zero = new Torque(0);

        private readonly decimal torqueInNewtonMeter;
        
        private Torque(decimal torqueInNewtonMeter)
        {
            this.torqueInNewtonMeter = torqueInNewtonMeter;
        }

        public static Torque operator +(Torque x, Torque y)
        {
            return new Torque(x.torqueInNewtonMeter + y.torqueInNewtonMeter);
        }

        public static Torque operator -(Torque x, Torque y)
        {
            return new Torque(x.torqueInNewtonMeter - y.torqueInNewtonMeter);
        }
        
        public static bool operator ==(Torque x, Torque y)
        {
            return x.torqueInNewtonMeter == y.torqueInNewtonMeter;
        }

        public static bool operator !=(Torque x, Torque y)
        {
            return !(x == y);
        }

        public static bool operator <(Torque x, Torque y)
        {
            return x.torqueInNewtonMeter < y.torqueInNewtonMeter;
        }

        public static bool operator >(Torque x, Torque y)
        {
            return x.torqueInNewtonMeter > y.torqueInNewtonMeter;
        }

        public static bool operator <=(Torque x, Torque y)
        {
            return x.torqueInNewtonMeter <= y.torqueInNewtonMeter;
        }

        public static bool operator >=(Torque x, Torque y)
        {
            return x.torqueInNewtonMeter >= y.torqueInNewtonMeter;
        }

        public static Torque operator /(Torque torque, int divisor)
        {
            Contract.Requires(torque != null);
            Contract.Ensures(Contract.Result<Torque>() != null);

            return new Torque(torque.torqueInNewtonMeter / divisor);
        }

        public static Torque operator *(Torque torque, int factor)
        {
            Contract.Requires(torque != null);
            Contract.Ensures(Contract.Result<Torque>() != null);

            return new Torque(torque.torqueInNewtonMeter * factor);
        }

        public static Torque operator *(int factor, Torque torque)
        {
            Contract.Requires(torque != null);
            Contract.Ensures(Contract.Result<Torque>() != null);

            return new Torque(torque.torqueInNewtonMeter * factor);
        }

        public static Torque operator /(Torque torque, long divisor)
        {
            Contract.Requires(torque != null);
            Contract.Ensures(Contract.Result<Torque>() != null);

            return new Torque(torque.torqueInNewtonMeter / divisor);
        }

        public static Torque operator *(Torque torque, long factor)
        {
            Contract.Requires(torque != null);
            Contract.Ensures(Contract.Result<Torque>() != null);

            return new Torque(torque.torqueInNewtonMeter * factor);
        }

        public static Torque operator *(long factor, Torque torque)
        {
            Contract.Requires(torque != null);
            Contract.Ensures(Contract.Result<Torque>() != null);

            return new Torque(torque.torqueInNewtonMeter * factor);
        }

        public static Torque operator /(Torque torque, double divisor)
        {
            Contract.Requires(torque != null);
            Contract.Ensures(Contract.Result<Torque>() != null);

            return new Torque(torque.torqueInNewtonMeter / new decimal(divisor));
        }

        public static Torque operator *(Torque torque, double factor)
        {
            Contract.Requires(torque != null);
            Contract.Ensures(Contract.Result<Torque>() != null);

            return new Torque(torque.torqueInNewtonMeter * new decimal(factor));
        }

        public static Torque operator *(double factor, Torque torque)
        {
            Contract.Requires(torque != null);
            Contract.Ensures(Contract.Result<Torque>() != null);

            return new Torque(torque.torqueInNewtonMeter * new decimal(factor));
        }

        public static Torque operator /(Torque torque, decimal divisor)
        {
            Contract.Requires(torque != null);
            Contract.Ensures(Contract.Result<Torque>() != null);

            return new Torque(torque.torqueInNewtonMeter / divisor);
        }

        public static Torque operator *(Torque torque, decimal factor)
        {
            Contract.Requires(torque != null);
            Contract.Ensures(Contract.Result<Torque>() != null);

            return new Torque(torque.torqueInNewtonMeter * factor);
        }

        public static Torque operator *(decimal factor, Torque torque)
        {
            Contract.Requires(torque != null);
            Contract.Ensures(Contract.Result<Torque>() != null);

            return new Torque(torque.torqueInNewtonMeter * factor);
        }

        public static Torque FromNewtonMeter(decimal torqueInNewtonMeter)
        {
            return new Torque(torqueInNewtonMeter);
        }

        public int CompareTo(Torque other)
        {
            return this.torqueInNewtonMeter.CompareTo(other.torqueInNewtonMeter);
        }

        public bool Equals(Torque other)
        {
            return this.torqueInNewtonMeter == other.torqueInNewtonMeter;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Torque))
            {
                return false;
            }

            Torque other = (Torque)obj;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.torqueInNewtonMeter.GetHashCode();
        }
    }
}