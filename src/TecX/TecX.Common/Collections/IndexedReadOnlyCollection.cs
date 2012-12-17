namespace TecX.Common.Collections
{
    using System.Collections;
    using System.Collections.Generic;

    public class IndexedReadOnlyCollection<T> : IIndexedReadOnlyCollection<T>
    {
        private readonly IList<T> inner;

        public IndexedReadOnlyCollection(IList<T> inner)
        {
            Guard.AssertNotNull(inner, "inner");
            this.inner = inner;
        }

        public T this[int index]
        {
            get
            {
                return this.inner[index];
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.inner.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}