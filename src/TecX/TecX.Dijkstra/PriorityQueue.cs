using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TecX.Dijkstra
{
    [Serializable]
    public class PriorityQueue<TValue, TPriority> : ICollection,
        IEnumerable<PriorityQueueItem<TValue, TPriority>>
    {
        #region Fields

        private PriorityQueueItem<TValue, TPriority>[] _items;

        private const int DefaultCapacity = 16;

        private int _capacity;

        private int _numItems;

        private Comparison<TPriority> _compare;

        #endregion Fields

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
            Init(initialCapacity, comparer.Compare);
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

        #region Properties

        public int Count
        {
            get
            {
                return _numItems;
            }
        }

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

        /// <summary>
        /// Enqueues the specified item in this queue
        /// </summary>
        /// <param name="item">The new item.</param>
        public void Enqueue(PriorityQueueItem<TValue, TPriority> item)
        {
            if (_numItems == _capacity)
            {
                // need to increase capacity
                // grow by 50 percent
                SetCapacity((3 * Capacity) / 2);
            }

            int i = _numItems;

            ++_numItems;

            while (i > 0 && _compare(_items[(i - 1) / 2].Priority, item.Priority) < 0)
            {
                _items[i] = _items[(i - 1) / 2];
                i = (i - 1) / 2;
            }

            _items[i] = item;

            //if (!VerifyQueue())
            //{
            //    Console.WriteLine("ERROR: Queue out of order!");
            //}
        }

        /// <summary>
        /// Enqueues the specified <paramref name="value"/> with the given <paramref name="priority"/>
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="priority">The priority.</param>
        public void Enqueue(TValue value, TPriority priority)
        {
            Enqueue(new PriorityQueueItem<TValue, TPriority>(value, priority));
        }

        /// <summary>
        ///  Function to check that the queue is coherent.
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

        /// <summary>
        /// Dequeues first element in this queue
        /// </summary>
        /// <returns>The element with the highest priority in this queue</returns>
        public PriorityQueueItem<TValue, TPriority> Dequeue()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("The queue is empty");
            }

            return RemoveAt(0);
        }

        /// <summary>
        /// Removes the item with the specified value from the queue.
        /// The passed equality comparison is used.
        /// </summary>
        /// <param name="item">The item to be removed.</param>
        /// <param name="comparer">An object that implements the IEqualityComparer interface
        /// for the type of item in the collection.</param>
        public void Remove(TValue item, IEqualityComparer comparer)
        {
            // need to find the PriorityQueueItem that has the Data value of o
            for (int index = 0; index < _numItems; ++index)
            {
                if (comparer.Equals(item, _items[index].Value))
                {
                    RemoveAt(index);
                    return;
                }
            }

            throw new InvalidOperationException("The specified item is not in the queue.");
        }

        /// <summary>
        /// Removes the item with the specified value from the queue.
        /// The default type comparison function is used.
        /// </summary>
        /// <param name="item">The item to be removed.</param>
        public void Remove(TValue item)
        {
            Remove(item, EqualityComparer<TValue>.Default);
        }

        /// <summary>
        /// Returns the first element in this queue without removing it
        /// </summary>
        /// <returns>The element with the highest priority in this queue</returns>
        public PriorityQueueItem<TValue, TPriority> Peek()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("The queue is empty");
            }

            return _items[0];
        }

        /// <summary>
        /// Clears this queue.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < _numItems; ++i)
            {
                _items[i] = default(PriorityQueueItem<TValue, TPriority>);
            }

            _numItems = 0;

            TrimExcess();
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
        /// Determines whether this queue contains the specified object.
        /// </summary>
        /// <param name="o">The object.</param>
        /// <returns>
        ///   <c>true</c> if this queue contains the specified object; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(TValue o)
        {
            foreach (PriorityQueueItem<TValue, TPriority> x in _items)
            {
                if (x.Value.Equals(o))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Copies the contents in this queue to an array starting at the specified <paramref name="arrayIndex"/>
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(PriorityQueueItem<TValue, TPriority>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("arrayIndex", "arrayIndex is less than 0.");
            }

            if (array.Rank > 1)
            {
                throw new ArgumentException("array is multidimensional.");
            }

            if (_numItems == 0)
            {
                return;
            }

            if (arrayIndex >= array.Length)
            {
                throw new ArgumentException("arrayIndex is equal to or greater than the length of the array.");
            }

            if (_numItems > (array.Length - arrayIndex))
            {
                throw new ArgumentException("The number of elements in the source ICollection is greater " +
                    "than the available space from arrayIndex to the end of the destination array.");
            }

            for (int i = 0; i < _numItems; i++)
            {
                array[arrayIndex + i] = _items[i];
            }
        }

        #region ICollection Members

        /// <inheritdoc/>
        public void CopyTo(Array array, int index)
        {
            CopyTo((PriorityQueueItem<TValue, TPriority>[])array, index);
        }

        /// <inheritdoc/>
        public bool IsSynchronized
        {
            get { return false; }
        }

        /// <inheritdoc/>
        public object SyncRoot
        {
            get { return _items.SyncRoot; }
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

        #endregion

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

            if (newCap < DefaultCapacity)
            {
                newCap = DefaultCapacity;
            }

            // throw exception if newCapacity < NumItems
            if (newCap < _numItems) throw new ArgumentOutOfRangeException("newCapacity", "New capacity is less than Count");

            _capacity = newCap;
            if (_items == null)
            {
                _items = new PriorityQueueItem<TValue, TPriority>[newCap];
                return;
            }

            // Resize the array.
            Array.Resize(ref _items, newCap);
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
