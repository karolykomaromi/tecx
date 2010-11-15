using System.Collections.Generic;
using System.Linq;

using TecX.Common;

namespace TecX.Agile.Peer
{
    public class RingBuffer<T>
    {
        private readonly int _capacity;
        private readonly IEqualityComparer<T> _comparer;
        private readonly List<T> _items;

        /// <summary>
        /// Initializes a new instance of the <see cref="RingBuffer{T}"/> class
        /// </summary>
        public RingBuffer(int capacity, IEqualityComparer<T> comparer)
        {
            Guard.AssertIsInRange(capacity, "capacity", 0, int.MaxValue);
            Guard.AssertNotNull(comparer, "comparer");

            _capacity = capacity;
            _comparer = comparer;
            _items = new List<T>(capacity);
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public void Add(T item)
        {
            Guard.AssertNotNull(item, "item");

            if(Count == _capacity)
            {
                _items.RemoveAt(0);
            }

            _items.Add(item);
        }

        public bool Contains(T candidate)
        {
            Guard.AssertNotNull(candidate, "candidate");

            bool contains = _items.Contains(candidate, _comparer);

            return contains;
        }
    }
}