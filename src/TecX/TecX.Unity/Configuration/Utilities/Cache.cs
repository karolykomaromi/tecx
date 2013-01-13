namespace TecX.Unity.Configuration.Utilities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using TecX.Common;

    public class Cache<TKey, TValue> : IEnumerable<TValue>
        where TValue : class
    {
        private readonly object locker = new object();

        private readonly IDictionary<TKey, TValue> values;

        private Func<TValue, TKey> getKey = delegate { throw new NotImplementedException(); };

        public Cache()
            : this(new Dictionary<TKey, TValue>())
        {
        }

        public Cache(Func<TKey, TValue> onMissing)
            : this(new Dictionary<TKey, TValue>(), onMissing)
        {
        }

        public Cache(IDictionary<TKey, TValue> dictionary, Func<TKey, TValue> onMissing)
        {
            Guard.AssertNotNull(dictionary, "dictionary");
            Guard.AssertNotNull(onMissing, "onMissing");

            this.values = dictionary;
            this.OnMissing = onMissing;
        }

        public Cache(IDictionary<TKey, TValue> dictionary)
            : this(dictionary, key => default(TValue))
        {
            this.values = dictionary;
        }

        public Func<TKey, TValue> OnMissing { get; set; }

        public Func<TValue, TKey> GetKey
        {
            get
            {
                return this.getKey;
            }

            set
            {
                Guard.AssertNotNull(value, "GetKey");

                this.getKey = value;
            }
        }

        public int Count
        {
            get { return this.values.Count; }
        }

        public TValue First
        {
            get
            {
                foreach (var pair in this.values)
                {
                    return pair.Value;
                }

                return null;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                if (!this.values.ContainsKey(key))
                {
                    lock (this.locker)
                    {
                        if (!this.values.ContainsKey(key))
                        {
                            TValue value = this.OnMissing(key);

                            // Check to make sure that the onMissing didn't cache this already
                            if (!this.values.ContainsKey(key))
                            {
                                this.values.Add(key, value);
                            }
                        }
                    }
                }

                return this.values[key];
            }

            set
            {
                if (this.values.ContainsKey(key))
                {
                    this.values[key] = value;
                }
                else
                {
                    this.values.Add(key, value);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<TValue>)this).GetEnumerator();
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return this.values.Values.GetEnumerator();
        }

        public void Each(Action<TValue> action)
        {
            Guard.AssertNotNull(action, "action");

            lock (this.locker)
            {
                foreach (var pair in this.values)
                {
                    action(pair.Value);
                }
            }
        }

        public void Each(Action<TKey, TValue> action)
        {
            Guard.AssertNotNull(action, "action");

            foreach (var pair in this.values)
            {
                action(pair.Key, pair.Value);
            }
        }

        public bool Has(TKey key)
        {
            Guard.AssertNotNull(key, "key");

            return this.values.ContainsKey(key);
        }

        public bool Exists(Predicate<TValue> predicate)
        {
            Guard.AssertNotNull(predicate, "predicate");

            bool returnValue = false;

            this.Each(delegate(TValue value) { returnValue |= predicate(value); });

            return returnValue;
        }

        public TValue Find(Predicate<TValue> predicate)
        {
            Guard.AssertNotNull(predicate, "predicate");

            foreach (var pair in this.values)
            {
                if (predicate(pair.Value))
                {
                    return pair.Value;
                }
            }

            return null;
        }

        public void Remove(TKey key)
        {
            Guard.AssertNotNull(key, "key");

            if (this.values.ContainsKey(key))
            {
                this.values.Remove(key);
            }
        }

        public void Clear()
        {
            this.values.Clear();
        }
    }
}
