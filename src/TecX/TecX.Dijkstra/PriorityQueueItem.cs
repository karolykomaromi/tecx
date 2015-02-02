using System;

namespace TecX.Dijkstra
{
    [Serializable]
    public struct PriorityQueueItem<TValue, TPriority>
    {
        private readonly TValue _value;
        private readonly TPriority _priority;

        public PriorityQueueItem(TValue value, TPriority priority)
        {
            _value = value;
            _priority = priority;
        }

        /// <summary>
        /// Gets the value of this PriorityQueueItem.
        /// </summary>
        public TValue Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Gets the priority associated with this PriorityQueueItem.
        /// </summary>
        public TPriority Priority
        {
            get { return _priority; }
        }
    }
}
