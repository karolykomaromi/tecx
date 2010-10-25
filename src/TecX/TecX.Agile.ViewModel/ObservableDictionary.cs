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
        private readonly Dictionary<TKey, TValue> _dict;

        public ObservableDictionary()
        {
            _dict = new Dictionary<TKey, TValue>();
        }

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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool ContainsKey(TKey key)
        {
            throw new NotImplementedException();
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
                NotifyCollectionChangedEventArgs args =
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, value);

                OnCollectionChanged(args);
            }

            return _dict.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            throw new NotImplementedException();
        }

        public TValue this[TKey key]
        {
            get 
            { 
                Guard.AssertNotNull(key, "key");

                TValue value;
                if(_dict.TryGetValue(key, out value))
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
                if(_dict.TryGetValue(key, out existing))
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
            get { throw new NotImplementedException(); }
        }

        public ICollection<TValue> Values
        {
            get { throw new NotImplementedException(); }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            Guard.AssertNotNull(args, "args");

            if (CollectionChanged != null)
            {
                CollectionChanged(this, args);
            }
        }
    }
}
