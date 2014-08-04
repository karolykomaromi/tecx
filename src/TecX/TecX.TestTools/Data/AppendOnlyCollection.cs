namespace TecX.TestTools.Data
{
    using System.Collections;
    using System.Collections.Generic;

    using TecX.Common;

    public class AppendOnlyCollection<T> : IAppendOnlyCollection<T>
    {
        private readonly ICollection<T> inner;

        public AppendOnlyCollection(ICollection<T> inner)
        {
            Guard.AssertNotNull(inner, "inner");

            this.inner = inner;
        }

        public AppendOnlyCollection()
            : this(new List<T>())
        {
        }

        public void Add(T item)
        {
            Guard.AssertNotNull(item, "item");

            this.inner.Add(item);
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