namespace TecX.EnumClasses
{
    using System.Linq;

    public abstract class Flags<T> : Enumeration where T : Flags<T>
    {
        public Flags(int value, string displayName, string name)
            : base(value, displayName, name)
        {
        }

        public static T operator |(Flags<T> x, Flags<T> y)
        {
            return x.Or(y.ToEnumeration());
        }

        public static T operator &(Flags<T> x, Flags<T> y)
        {
            var f1 = x.ToArray();
            var f2 = y.ToArray();

            var f3 = f1.Intersect(f2).ToArray();

            if (f3.Length == 0)
            {
                return Enumeration.FromValue<T>(0);
            }

            if (f3.Length == 1)
            {
                return f3[0];
            }

            var or = f3[0].Or(f3[1]);

            for (int i = 2; i < f3.Length; i++)
            {
                or = or.Or(f3[i]);
            }

            return or;
        }

        protected internal virtual T[] ToArray()
        {
            return new[] { this.ToEnumeration() };
        }

        protected abstract T ToEnumeration();

        protected abstract T Or(T x);
    }
}