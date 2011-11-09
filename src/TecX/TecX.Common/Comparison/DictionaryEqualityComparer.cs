namespace TecX.Common.Comparison
{
    using System.Collections.Generic;

    public class DictionaryEqualityComparer<TKey, TValue> : EqualityComparer<IDictionary<TKey, TValue>>
    {
        /// <inheritdoc/>
        public override bool Equals(IDictionary<TKey, TValue> x, IDictionary<TKey, TValue> y)
        {
            if (Compare.AreBothNull(x, y))
            {
                return true;
            }

            if (Compare.IsExactlyOneNull(x, y))
            {
                return false;
            }

            if (x.Count != y.Count)
            {
                return false;
            }

            foreach (TKey key in x.Keys)
            {
                // one dictionary contains elements the other does not contain -> not equal
                if (!x.ContainsKey(key) || !y.ContainsKey(key))
                {
                    return false;
                }

                TValue valueFirst = x[key];
                TValue valueScnd = y[key];

                if (!Compare.AreEqual(valueFirst, valueScnd))
                {
                    return false;
                }
            }

            return true;
        }

        /// <inheritdoc/>
        public override int GetHashCode(IDictionary<TKey, TValue> obj)
        {
            Guard.AssertNotNull(obj, "obj");

            int hash = obj.GetHashCode();

            return hash;
        }
    }
}