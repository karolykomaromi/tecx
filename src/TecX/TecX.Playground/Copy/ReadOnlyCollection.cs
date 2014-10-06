using System.Collections;
using System.Collections.Generic;

namespace TecX.Playground.Copy
{
    public class ReadOnlyCollection<T> : IReadonlyCollection<T>
    {
        private readonly System.Collections.ObjectModel.ReadOnlyCollection<T> _Inner;

        public ReadOnlyCollection(IList<T> inner)
        {
            _Inner = new System.Collections.ObjectModel.ReadOnlyCollection<T>(inner);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _Inner.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count { get { return _Inner.Count; } }
    }
}