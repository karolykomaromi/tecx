namespace TecX.Common.Collections
{
    using System.Collections;
    using System.Collections.Generic;

    public class KeyedReadOnlyCollection<TKey, TValue> : IKeyedReadOnlyCollections<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> inner;

        public KeyedReadOnlyCollection(IDictionary<TKey, TValue> inner)
        {
            Guard.AssertNotNull(inner, "inner");
            this.inner = inner;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.inner.TryGetValue(key, out value);
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return this.inner.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}