namespace Hydra.Infrastructure
{
    using System;
    using System.Linq;

    public abstract class Flags<T> : Enumeration<T> where T : Flags<T>
    {
        protected Flags(string name, int sortKey)
            : base(name, sortKey)
        {
        }

        public override int Value
        {
            get
            {
                if (object.ReferenceEquals(EnumerationValues.Values.First(), this))
                {
                    return 0;
                }

                int value = (int)Math.Pow(2, base.Value - 1);

                return value;
            }
        }

        public abstract T Or(T other);
    }
}