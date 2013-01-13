using System;
using System.Diagnostics;

namespace TecX.Dijkstra
{
    /// <summary>
    /// Represents an element in a <see cref="PriorityQueue{TValue,TPriority}"/>
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <typeparam name="TPriority">The type of the priority.</typeparam>
    [Serializable]
    [DebuggerDisplay("{Priority} {Value}")]
    public struct PriorityQueueItem<TValue, TPriority>
    {
        private readonly TValue _value;

        private readonly TPriority _priority;

        /// <summary>
        /// Gets the value.
        /// </summary>
        public TValue Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Gets the priority.
        /// </summary>
        public TPriority Priority
        {
            get { return _priority; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityQueueItem&lt;TValue, TPriority&gt;"/> struct.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <param name="pri">The pri.</param>
        public PriorityQueueItem(TValue val, TPriority pri)
        {
            _value = val;
            _priority = pri;
        }
    }
}
