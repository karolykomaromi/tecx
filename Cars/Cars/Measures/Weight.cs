namespace Cars.Measures
{
    using System;
    using System.Diagnostics.Contracts;

    public class Weight : IEquatable<Weight>, IComparable<Weight>
    {
        public readonly static Weight Zero = new Weight(0);

        private readonly decimal weightInGrams = 0;

        private Weight(decimal weightInGrams)
        {
            this.weightInGrams = weightInGrams;
        }

        public static Weight operator +(Weight w1, Weight w2)
        {
            Contract.Requires(w1 != null);
            Contract.Requires(w2 != null);
            Contract.Ensures(Contract.Result<Weight>() != null);

            return new Weight(w1.weightInGrams + w2.weightInGrams);
        }

        public static Weight operator -(Weight w1, Weight w2)
        {
            Contract.Requires(w1 != null);
            Contract.Requires(w2 != null);
            Contract.Ensures(Contract.Result<Weight>() != null);

            return new Weight(w1.weightInGrams - w2.weightInGrams);
        }

        public static bool operator ==(Weight x, Weight y)
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

            return x.weightInGrams == y.weightInGrams;
        }

        public static bool operator !=(Weight x, Weight y)
        {
            return !(x == y);
        }

        public static bool operator <(Weight x, Weight y)
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

            return x.weightInGrams < y.weightInGrams;
        }

        public static bool operator >(Weight x, Weight y)
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

            return x.weightInGrams > y.weightInGrams;
        }

        public static bool operator <=(Weight x, Weight y)
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

            return x.weightInGrams <= y.weightInGrams;
        }

        public static bool operator >=(Weight x, Weight y)
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

            return x.weightInGrams >= y.weightInGrams;
        }

        public static Weight operator /(Weight weight, int divisor)
        {
            Contract.Requires(weight != null);
            Contract.Ensures(Contract.Result<Weight>() != null);

            return new Weight(weight.weightInGrams / divisor);
        }

        public static Weight operator *(Weight weight, int factor)
        {
            Contract.Requires(weight != null);
            Contract.Ensures(Contract.Result<Weight>() != null);

            return new Weight(weight.weightInGrams * factor);
        }

        public static Weight operator *(int factor, Weight weight)
        {
            Contract.Requires(weight != null);
            Contract.Ensures(Contract.Result<Weight>() != null);

            return new Weight(weight.weightInGrams * factor);
        }

        public static Weight operator /(Weight weight, long divisor)
        {
            Contract.Requires(weight != null);
            Contract.Ensures(Contract.Result<Weight>() != null);

            return new Weight(weight.weightInGrams / divisor);
        }

        public static Weight operator *(Weight weight, long factor)
        {
            Contract.Requires(weight != null);
            Contract.Ensures(Contract.Result<Weight>() != null);

            return new Weight(weight.weightInGrams * factor);
        }

        public static Weight operator *(long factor, Weight weight)
        {
            Contract.Requires(weight != null);
            Contract.Ensures(Contract.Result<Weight>() != null);

            return new Weight(weight.weightInGrams * factor);
        }

        public static Weight operator /(Weight weight, double divisor)
        {
            Contract.Requires(weight != null);
            Contract.Ensures(Contract.Result<Weight>() != null);

            return new Weight(weight.weightInGrams / new decimal(divisor));
        }

        public static Weight operator *(Weight weight, double factor)
        {
            Contract.Requires(weight != null);
            Contract.Ensures(Contract.Result<Weight>() != null);

            return new Weight(weight.weightInGrams * new decimal(factor));
        }

        public static Weight operator *(double factor, Weight weight)
        {
            Contract.Requires(weight != null);
            Contract.Ensures(Contract.Result<Weight>() != null);

            return new Weight(weight.weightInGrams * new decimal(factor));
        }

        public static Weight operator /(Weight weight, decimal divisor)
        {
            Contract.Requires(weight != null);
            Contract.Ensures(Contract.Result<Weight>() != null);

            return new Weight(weight.weightInGrams / divisor);
        }

        public static Weight operator *(Weight weight, decimal factor)
        {
            Contract.Requires(weight != null);
            Contract.Ensures(Contract.Result<Weight>() != null);

            return new Weight(weight.weightInGrams * factor);
        }

        public static Weight operator *(decimal factor, Weight weight)
        {
            Contract.Requires(weight != null);
            Contract.Ensures(Contract.Result<Weight>() != null);

            return new Weight(weight.weightInGrams * factor);
        }

        public static Weight FromGrams(decimal weightInGrams)
        {
            return new Weight(weightInGrams);
        }

        public static Weight FromKilograms(decimal weightInKilograms)
        {
            return new Weight(weightInKilograms * 1000);
        }

        public int CompareTo(Weight other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return 1;
            }

            return this.weightInGrams.CompareTo(other.weightInGrams);
        }

        public bool Equals(Weight other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }

            return this.weightInGrams == other.weightInGrams;
        }

        public override bool Equals(object obj)
        {
            Weight other = obj as Weight;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.weightInGrams.GetHashCode();
        }
    }
}