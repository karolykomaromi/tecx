using System;
using System.Collections;
using System.Collections.Generic;

using TecX.Common;

namespace TecX.Dijkstra
{
    [Serializable]
    public class PriorityQueue<TValue, TPriority> : ICollection<PriorityQueueItem<TValue, TPriority>>, IPriorityQueue<TValue, TPriority>
    {
        #region Constants

        private const int DefaultCapacity = 16;

        #endregion Constants

        #region Fields

        private PriorityQueueItem<TValue, TPriority>[] _items;

        private int _capacity;

        private int _numItems;

        private Comparison<TPriority> _compare;

        #endregion Fields

        #region Properties

        public int Capacity
        {
            get
            {
                return _items.Length;
            }
            set
            {
                SetCapacity(value);
            }
        }

        #endregion Properties

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the PriorityQueue class that is empty,
        /// has the default initial capacity, and uses the default IComparer.
        /// </summary>
        public PriorityQueue()
            : this(DefaultCapacity, Comparer<TPriority>.Default)
        {
        }

        public PriorityQueue(int initialCapacity)
            : this(initialCapacity, Comparer<TPriority>.Default)
        {
        }

        public PriorityQueue(IComparer<TPriority> comparer)
            : this(DefaultCapacity, comparer)
        {
        }

        public PriorityQueue(int initialCapacity, IComparer<TPriority> comparer)
        {
            Init(initialCapacity, new Comparison<TPriority>(comparer.Compare));
        }

        public PriorityQueue(Comparison<TPriority> comparison)
            : this(DefaultCapacity, comparison)
        {
        }

        public PriorityQueue(int initialCapacity, Comparison<TPriority> comparison)
        {
            Init(initialCapacity, comparison);
        }

        #endregion c'tor

        /// <summary>
        /// Enqueues the specified <paramref name="value"/> with the given <paramref name="priority"/>
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="priority">The value's priority.</param>
        public void Enqueue(TValue value, TPriority priority)
        {
            Enqueue(new PriorityQueueItem<TValue, TPriority>(value, priority));
        }

        /// <summary>
        /// Enqueues the specified <see cref="PriorityQueueItem{TValue,TPriority}"/>
        /// </summary>
        /// <param name="newItem">The new item.</param>
        public void Enqueue(PriorityQueueItem<TValue, TPriority> newItem)
        {
            if (_numItems == _capacity)
            {
                // need to increase capacity
                // grow by 50 percent
                SetCapacity((3 * Capacity) / 2);
            }

            int i = _numItems;
            ++_numItems;
            while ((i > 0) && (_compare(_items[(i - 1) / 2].Priority, newItem.Priority) < 0))
            {
                _items[i] = _items[(i - 1) / 2];
                i = (i - 1) / 2;
            }
            _items[i] = newItem;

            //if (!VerifyQueue())
            //{
            //    Console.WriteLine("ERROR: Queue out of order!");
            //}
        }

        /// <summary>
        /// Returns and removes the head of the queue, i.e. the item with the highest priority
        /// </summary>
        /// <returns>The item with the highest priority in this queue</returns>
        public PriorityQueueItem<TValue, TPriority> Dequeue()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("The queue is empty");
            }

            return RemoveAt(0);
        }

        /// <summary>
        /// Returns the head of the queue but does not remove it from the queue.
        /// </summary>
        /// <returns>The item with the highest priority in this queue</returns>
        public PriorityQueueItem<TValue, TPriority> Peek()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("The queue is empty");
            }

            return _items[0];
        }
        
        /// <summary>
        /// Removes the item with the specified value from the queue.
        /// The default type comparison function is used.
        /// </summary>
        /// <param name="item">The item to be removed.</param>
        public bool Remove(TValue item)
        {
            return Remove(item, EqualityComparer<TValue>.Default);
        }

        /// <summary>
        /// Removes the item with the specified value from the queue.
        /// The passed equality comparison is used.
        /// </summary>
        /// <param name="item">The item to be removed.</param>
        /// <param name="comparer">An object that implements the <see cref="IEqualityComparer{T}"/> interface
        /// for the type of item in the collection.</param>
        public bool Remove(TValue item, IEqualityComparer<TValue> comparer)
        {
            // need to find the PriorityQueueItem that has the Data value of item
            for (int index = 0; index < _numItems; ++index)
            {
                if (comparer.Equals(item, _items[index].Value))
                {
                    RemoveAt(index);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Set the capacity to the actual number of items, if the current
        /// number of items is less than 90 percent of the current capacity.
        /// </summary>
        public void TrimExcess()
        {
            if (_numItems < (float)0.9 * _capacity)
            {
                SetCapacity(_numItems);
            }
        }

        /// <summary>
        /// Determines whether the <see cref="PriorityQueue{TValue,TPriority}"></see> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="PriorityQueue{TValue,TPriority}"></see>.</param>
        /// <returns>
        /// <c>true</c> if item is found in the <see cref="PriorityQueue{TValue,TPriority}"></see>; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(TValue item)
        {
            foreach (PriorityQueueItem<TValue, TPriority> x in _items)
            {
                if (x.Value.Equals(item))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks that the queue is coherent.
        /// </summary>
        /// <returns><c>true</c> if this queue is coherent; otherwise <c>false</c></returns>
        public bool VerifyQueue()
        {
            int i = 0;

            while (i < _numItems / 2)
            {
                int leftChild = (2 * i) + 1;
                int rightChild = leftChild + 1;

                if (_compare(_items[i].Priority, _items[leftChild].Priority) < 0)
                {
                    return false;
                }

                if (rightChild < _numItems && _compare(_items[i].Priority, _items[rightChild].Priority) < 0)
                {
                    return false;
                }

                ++i;
            }

            return true;
        }

        #region ICollection Members

        /// <inheritdoc/>
        public void Clear()
        {
            for (int i = 0; i < _numItems; ++i)
            {
                _items[i] = default(PriorityQueueItem<TValue, TPriority>);
            }

            _numItems = 0;

            TrimExcess();
        }

        /// <inheritdoc/>
        public int Count
        {
            get
            {
                return _numItems;
            }
        }
        
        /// <inheritdoc/>
        void ICollection<PriorityQueueItem<TValue, TPriority>>.Add(PriorityQueueItem<TValue, TPriority> item)
        {
            Enqueue(item);
        }

        /// <inheritdoc/>
        void ICollection<PriorityQueueItem<TValue, TPriority>>.CopyTo(PriorityQueueItem<TValue, TPriority>[] array, int arrayIndex)
        {
            Guard.AssertNotNull(array, "array");

            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("arrayIndex", "arrayIndex is less than 0.");

            if (array.Rank > 1)
                throw new ArgumentException("array is multidimensional.");

            if (_numItems == 0)
                return;

            if (arrayIndex >= array.Length)
                throw new ArgumentException("arrayIndex is equal to or greater than the length of the array.");

            if (_numItems > (array.Length - arrayIndex))
                throw new ArgumentException("The number of elements in the source ICollection is greater than the " +
                    "available space from arrayIndex to the end of the destination array.");

            for (int i = 0; i < _numItems; i++)
            {
                array[arrayIndex + i] = _items[i];
            }
        }

        /// <inheritdoc/>
        bool ICollection<PriorityQueueItem<TValue, TPriority>>.Contains(PriorityQueueItem<TValue, TPriority> item)
        {
            return Contains(item.Value);
        }

        /// <inheritdoc/>
        bool ICollection<PriorityQueueItem<TValue, TPriority>>.Remove(PriorityQueueItem<TValue, TPriority> item)
        {
            return Remove(item.Value);
        }

        /// <inheritdoc/>
        bool ICollection<PriorityQueueItem<TValue, TPriority>>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public IEnumerator<PriorityQueueItem<TValue, TPriority>> GetEnumerator()
        {
            for (int i = 0; i < _numItems; i++)
            {
                yield return _items[i];
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion ICollection Members

        #region Helper

        private void Init(int initialCapacity, Comparison<TPriority> comparison)
        {
            _numItems = 0;
            _compare = comparison;
            SetCapacity(initialCapacity);
        }

        private void SetCapacity(int newCapacity)
        {
            int newCap = newCapacity;
            if (newCap < DefaultCapacity) newCap = DefaultCapacity;

            // throw exception if newCapacity < NumItems
            if (newCap < _numItems) throw new ArgumentOutOfRangeException("newCapacity", "New capacity is less than Count");

            this._capacity = newCap;
            if (_items == null)
            {
                _items = new PriorityQueueItem<TValue, TPriority>[newCap];
                return;
            }

            // Resize the array.
            Array.Resize<PriorityQueueItem<TValue, TPriority>>(ref _items, newCap);
        }

        private PriorityQueueItem<TValue, TPriority> RemoveAt(int index)
        {
            PriorityQueueItem<TValue, TPriority> o = _items[index];
            --_numItems;
            // move the last item to fill the hole
            PriorityQueueItem<TValue, TPriority> tmp = _items[_numItems];
            // If you forget to clear this, you have a potential memory leak.
            _items[_numItems] = default(PriorityQueueItem<TValue, TPriority>);
            if (_numItems > 0 && index != _numItems)
            {
                // If the new item is greater than its parent, bubble up.
                int i = index;
                int parent = (i - 1) / 2;
                while (_compare(tmp.Priority, _items[parent].Priority) > 0)
                {
                    _items[i] = _items[parent];
                    i = parent;
                    parent = (i - 1) / 2;
                }

                // if i == index, then we didn't move the item up
                if (i == index)
                {
                    // bubble down ...
                    while (i < (_numItems) / 2)
                    {
                        int j = (2 * i) + 1;
                        if ((j < _numItems - 1) && (_compare(_items[j].Priority, _items[j + 1].Priority) < 0))
                        {
                            ++j;
                        }
                        if (_compare(_items[j].Priority, tmp.Priority) <= 0)
                        {
                            break;
                        }
                        _items[i] = _items[j];
                        i = j;
                    }
                }
                // Be sure to store the item in its place.
                _items[i] = tmp;
            }
            //if (!VerifyQueue())
            //{
            //    Console.WriteLine("ERROR: Queue out of order!");
            //}
            return o;
        }

        #endregion Helper
    }
}
