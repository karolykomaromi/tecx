namespace Hydra.Infrastructure
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public class EqualityComparerAdapter<T> : IEqualityComparer
    {
        private readonly IEqualityComparer<T> comparer;

        public EqualityComparerAdapter(IEqualityComparer<T> comparer)
        {
            Contract.Requires(comparer != null);

            this.comparer = comparer;
        }

        public IEqualityComparer<T> Comparer
        {
            get { return this.comparer; }
        }

        bool IEqualityComparer.Equals(object x, object y)
        {
            if (!(x is T) || !(y is T))
            {
                return false;
            }

            T gx = (T)x;
            T gy = (T)y;

            return this.Comparer.Equals(gx, gy);
        }

        int IEqualityComparer.GetHashCode(object obj)
        {
            if (!(obj is T))
            {
                return 0;
            }

            return this.Comparer.GetHashCode((T)obj);
        }
    }
}