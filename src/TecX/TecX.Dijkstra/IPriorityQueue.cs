using System.Collections.Generic;

namespace TecX.Dijkstra
{
    public interface IPriorityQueue<TValue, TPriority>
    {
        /// <summary>
        /// Enqueues the specified <paramref name="value"/> with the given <paramref name="priority"/>
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="priority">The value's priority.</param>
        void Enqueue(TValue value, TPriority priority);

        /// <summary>
        /// Enqueues the specified <see cref="PriorityQueueItem{TValue,TPriority}"/>
        /// </summary>
        /// <param name="newItem">The new item.</param>
        void Enqueue(PriorityQueueItem<TValue, TPriority> newItem);

        /// <summary>
        /// Returns and removes the head of the queue, i.e. the item with the highest priority
        /// </summary>
        /// <returns>The item with the highest priority in this queue</returns>
        PriorityQueueItem<TValue, TPriority> Dequeue();

        /// <summary>
        /// Returns the head of the queue but does not remove it from the queue.
        /// </summary>
        /// <returns>The item with the highest priority in this queue</returns>
        PriorityQueueItem<TValue, TPriority> Peek();

        /// <summary>
        /// Set the capacity to the actual number of items, if the current
        /// number of items is less than 90 percent of the current capacity.
        /// </summary>
        void TrimExcess();

        /// <summary>
        /// Determines whether the <see cref="PriorityQueue{TValue,TPriority}"></see> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="PriorityQueue{TValue,TPriority}"></see>.</param>
        /// <returns>
        /// <c>true</c> if item is found in the <see cref="PriorityQueue{TValue,TPriority}"></see>; otherwise, <c>false</c>.
        /// </returns>
        bool Contains(TValue item);

        /// <summary>
        /// Removes all items from the <see cref="IPriorityQueue{TValue,TPriority}"/>.
        /// </summary>
        void Clear();

        /// <summary>
        /// Removes the item with the specified value from the queue.
        /// The default type comparison function is used.
        /// </summary>
        /// <param name="item">The item to be removed.</param>
        bool Remove(TValue item);

        /// <summary>
        /// Removes the item with the specified value from the queue.
        /// The passed equality comparison is used.
        /// </summary>
        /// <param name="item">The item to be removed.</param>
        /// <param name="comparer">An object that implements the <see cref="IEqualityComparer{T}"/> interface
        /// for the type of item in the collection.</param>
        bool Remove(TValue item, IEqualityComparer<TValue> comparer);
    }
}