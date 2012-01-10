using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using TecX.Common;
using TecX.Common.Extensions.Error;

namespace TecX.Agile.ViewModel
{
    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, INotifyCollectionChanged
    {
        #region Fields

        private readonly Dictionary<TKey, TValue> _dict;

        #endregion Fields

        #region c'tor

        public ObservableDictionary()
        {
            _dict = new Dictionary<TKey, TValue>();
        }

        #endregion c'tor

        #region Implementation of IDictionary<TKey,TValue>

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Guard.AssertNotNull(item, "item");
            Guard.AssertNotNull(item.Key, "item.Key");

            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            NotifyCollectionChangedEventArgs args =
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);

            OnCollectionChanged(args);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            Guard.AssertNotNull(item, "item");

            return _dict.Contains(item);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)_dict).CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get
            {
                return ((ICollection<KeyValuePair<TKey, TValue>>)_dict).IsReadOnly;
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            Guard.AssertNotNull(item, "item");
            Guard.AssertNotNull(item.Key, "item.Key");

            return Remove(item.Key);
        }

        public int Count
        {
            get { return _dict.Count; }
        }

        public bool ContainsKey(TKey key)
        {
            Guard.AssertNotNull(key, "key");

            return _dict.ContainsKey(key);
        }

        public void Add(TKey key, TValue value)
        {
            Guard.AssertNotNull(key, "key");

            _dict.Add(key, value);

            NotifyCollectionChangedEventArgs args =
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value);

            OnCollectionChanged(args);
        }

        public bool Remove(TKey key)
        {
            Guard.AssertNotNull(key, "key");

            TValue value;
            if (_dict.TryGetValue(key, out value))
            {
                _dict.Remove(key);

                NotifyCollectionChangedEventArgs args =
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, value);

                OnCollectionChanged(args);

                return true;
            }

            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dict.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get
            {
                Guard.AssertNotNull(key, "key");

                TValue value;
                if (_dict.TryGetValue(key, out value))
                {
                    return value;
                }

                throw new InvalidOperationException(string.Format("No item with key {0} in this dictionary", key))
                    .WithAdditionalInfo("key", key);
            }
            set
            {
                Guard.AssertNotNull(key, "key");

                TValue existing;
                if (_dict.TryGetValue(key, out existing))
                {
                    NotifyCollectionChangedEventArgs args =
                        new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, existing);

                    OnCollectionChanged(args);
                }

                _dict[key] = value;
            }
        }

        public ICollection<TKey> Keys
        {
            get { return _dict.Keys; }
        }

        public ICollection<TValue> Values
        {
            get { return _dict.Values; }
        }

        #endregion Implementation of IDictionary<TKey,TValue>

        #region Implementation of INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            Guard.AssertNotNull(args, "args");

            if (CollectionChanged != null)
            {
                CollectionChanged(this, args);
            }
        }

        #endregion Implementation of INotifyCollectionChanged
    }
}