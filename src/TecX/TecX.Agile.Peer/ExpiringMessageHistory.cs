using System;
using System.Collections.Generic;

using TecX.Common;
using TecX.Common.Time;

namespace TecX.Agile.Peer
{
    public class ExpiringMessageHistory<T> : MessageHistory<T>
    {
        private readonly TimeSpan _timeout;
        private readonly IEqualityComparer<T> _comparer;
        private readonly List<BufferItem<T>> _items;

        public override int Count
        {
            get { return _items.Count; }
        }

        public ExpiringMessageHistory(TimeSpan timeout, IEqualityComparer<T> comparer)
        {
            Guard.AssertNotNull(comparer, "comparer");

            _timeout = timeout;
            _comparer = comparer;
            _items = new List<BufferItem<T>>();
        }

        public override void Add(T item)
        {
            Guard.AssertNotNull(item, "item");

            _items.Add(new BufferItem<T>(item, new SlidingLease(this._timeout)));
        }

        public override bool Contains(T candidate)
        {
            Guard.AssertNotNull(candidate, "candidate");

            foreach (var item in _items.ToArray())
            {
                if(item.Lease.IsExpired)
                {
                    _items.Remove(item);
                    continue;
                }

                if (_comparer.Equals(item.Item, candidate))
                {
                    return true;
                }
            }

            return false;
        }

        public override bool Remove(T item)
        {
            Guard.AssertNotNull(item, "item");

            int index = _items.FindIndex(i => _comparer.Equals(i.Item, item));

            if(index != -1)
            {
                _items.RemoveAt(index);
                return true;
            }

            return false;
        }

        private class BufferItem<TItem>
        {
            private readonly TItem _item;
            private readonly ILease _lease;

            public ILease Lease
            {
                get { return _lease; }
            }

            public TItem Item
            {
                get { return _item; }
            }

            public BufferItem(TItem item, ILease lease)
            {
                Guard.AssertNotNull(item, "item");
                Guard.AssertNotNull(lease, "lease");

                _item = item;
                _lease = lease;
            }
        }
    }
}