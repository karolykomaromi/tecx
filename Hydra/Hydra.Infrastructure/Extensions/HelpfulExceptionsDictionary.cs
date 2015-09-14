namespace Hydra.Infrastructure.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public class HelpfulExceptionsDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> target;

        public HelpfulExceptionsDictionary(IDictionary<TKey, TValue> target)
        {
            Contract.Requires(target != null);

            this.target = target;
        }

        public int Count
        {
            get { return this.target.Count; }
        }

        public bool IsReadOnly
        {
            get { return this.target.IsReadOnly; }
        }

        public ICollection<TKey> Keys
        {
            get { return this.target.Keys; }
        }

        public ICollection<TValue> Values
        {
            get { return this.target.Values; }
        }

        public TValue this[TKey key]
        {
            get
            {
                try
                {
                    return this.target[key];
                }
                catch (KeyNotFoundException ex)
                {
                    throw new MissingKeyException(ex, key);
                }
            }

            set
            {
                this.target[key] = value;
            }
        }

        public void Add(TKey key, TValue value)
        {
            try
            {
                this.target.Add(key, value);
            }
            catch (ArgumentException ex)
            {
                throw new DuplicateKeyException(ex.ParamName, ex, key);
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            try
            {
                this.target.Add(item);
            }
            catch (ArgumentException ex)
            {
                throw new DuplicateKeyException(ex.ParamName, ex, item.Key);
            }
        }

        public void Clear()
        {
            this.target.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.target.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return this.target.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            this.target.CopyTo(array, arrayIndex);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.target.GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            return this.target.Remove(key);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return this.target.Remove(item);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.target.TryGetValue(key, out value);
        }
    }
}