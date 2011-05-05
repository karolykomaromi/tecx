﻿using System;
using System.Collections;
using System.Collections.Generic;

using TecX.Common;
using TecX.Unity.Configuration.Extensions;

namespace TecX.Unity.Configuration.Common
{
    public class Cache<TKey, TValue> : IEnumerable<TValue> 
        where TValue : class
    {
        #region Fields

        private readonly object _locker = new object();
        private readonly IDictionary<TKey, TValue> _values;

        private Func<TValue, TKey> _getKey = delegate { throw new NotImplementedException(); };

        private Func<TKey, TValue> _onMissing = delegate(TKey key)
            {
                string message = string.Format("Key '{0}' could not be found", key);
                throw new KeyNotFoundException(message);
            };

        #endregion Fields

        #region c'tor

        public Cache()
            : this(new Dictionary<TKey, TValue>())
        {
        }

        public Cache(Func<TKey, TValue> onMissing)
            : this(new Dictionary<TKey, TValue>(), onMissing)
        {
        }

        public Cache(IDictionary<TKey, TValue> dictionary, Func<TKey, TValue> onMissing)
            : this(dictionary)
        {
            _onMissing = onMissing;
        }

        public Cache(IDictionary<TKey, TValue> dictionary)
        {
            _values = dictionary;
        }

        #endregion c'tor

        public Func<TKey, TValue> OnMissing
        {
            set { _onMissing = value; }
        }

        public Func<TValue, TKey> GetKey
        {
            get
            {
                return _getKey;
            }
            set
            {
                Guard.AssertNotNull(value, "GetKey");
                _getKey = value;
            }
        }

        public int Count
        {
            get { return _values.Count; }
        }

        public TValue First
        {
            get
            {
                foreach (var pair in _values)
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
                if (!_values.ContainsKey(key))
                {
                    lock (_locker)
                    {
                        if (!_values.ContainsKey(key))
                        {
                            TValue value = _onMissing(key);
                            //Check to make sure that the onMissing didn't cache this already
                            if (!_values.ContainsKey(key))
                            {
                                _values.Add(key, value);
                            }
                        }
                    }
                }

                return _values[key];
            }
            set
            {
                if (_values.ContainsKey(key))
                {
                    _values[key] = value;
                }
                else
                {
                    _values.Add(key, value);
                }
            }
        }

        #region IEnumerable<TValue> Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<TValue>)this).GetEnumerator();
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return _values.Values.GetEnumerator();
        }

        #endregion IEnumerable<TValue> Members

        public void Fill(TKey key, TValue value)
        {
            Guard.AssertNotNull(key, "key");

            if (_values.ContainsKey(key))
            {
                return;
            }

            _values.Add(key, value);
        }

        public bool TryRetrieve(TKey key, out TValue value)
        {
            Guard.AssertNotNull(key, "key");

            value = default(TValue);

            if (_values.ContainsKey(key))
            {
                value = _values[key];
                return true;
            }

            return false;
        }

        public void Each(Action<TValue> action)
        {
            Guard.AssertNotNull(action, "action");

            lock (_locker)
            {
                foreach (var pair in _values)
                {
                    action(pair.Value);
                }
            }
        }

        public void Each(Action<TKey, TValue> action)
        {
            Guard.AssertNotNull(action, "action");

            foreach (var pair in _values)
            {
                action(pair.Key, pair.Value);
            }
        }

        public bool Has(TKey key)
        {
            Guard.AssertNotNull(key, "key");

            return _values.ContainsKey(key);
        }

        public bool Exists(Predicate<TValue> predicate)
        {
            Guard.AssertNotNull(predicate, "predicate");

            bool returnValue = false;

            Each(delegate(TValue value) { returnValue |= predicate(value); });

            return returnValue;
        }

        public TValue Find(Predicate<TValue> predicate)
        {
            Guard.AssertNotNull(predicate, "predicate");

            foreach (var pair in _values)
            {
                if (predicate(pair.Value))
                {
                    return pair.Value;
                }
            }

            return null;
        }

        public TValue[] GetAll()
        {
            var returnValue = new TValue[Count];
            _values.Values.CopyTo(returnValue, 0);

            return returnValue;
        }

        public void Remove(TKey key)
        {
            Guard.AssertNotNull(key, "key");

            if (_values.ContainsKey(key))
            {
                _values.Remove(key);
            }
        }

        public void Clear()
        {
            _values.Clear();
        }

        public Cache<TKey, TValue> Clone()
        {
            var clone = new Cache<TKey, TValue>(_onMissing);
            _values.Each(pair => clone[pair.Key] = pair.Value);

            return clone;
        }

        public void WithValue(TKey key, Action<TValue> action)
        {
            Guard.AssertNotNull(key, "key");
            Guard.AssertNotNull(action, "action");

            if (_values.ContainsKey(key))
            {
                action(this[key]);
            }
        }
    }
}