namespace TecX.Unity.Tracking
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using TecX.Common;

    public class HierarchicalDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> top;

        private readonly IDictionary<TKey, TValue> down;

        public HierarchicalDictionary(IDictionary<TKey, TValue> top, IDictionary<TKey, TValue> down)
        {
            Guard.AssertNotNull(top, "top");
            Guard.AssertNotNull(down, "down");

            this.top = top;
            this.down = down;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.down.Add(item);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return this.down.Remove(item) || this.top.Remove(item);
        }

        public int Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsReadOnly
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool ContainsKey(TKey key)
        {
            return this.down.ContainsKey(key) || this.top.ContainsKey(key);
        }

        public void Add(TKey key, TValue value)
        {
            this.down.Add(key, value);
        }

        public bool Remove(TKey key)
        {
            return this.down.Remove(key) || this.top.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (this.down.TryGetValue(key, out value))
            {
                return true;
            }

            if (this.top.TryGetValue(key, out value))
            {
                return true;
            }

            value = default(TValue);
            return false;
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                if (this.TryGetValue(key, out value))
                {
                    return value;
                }

                throw new KeyNotFoundException(string.Format("Key {0} not found.", key));
            }

            set
            {
                this.down[key] = value;
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}