namespace Hydra.Infrastructure.Test
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Xunit;

    public class DictionaryTests
    {
        [Fact]
        public void Should_Not_Throw_Totally_Useless_Exception_On_Duplicate_Key()
        {
            IDictionary<int, string> sut = new Dictionary<int, string>().Wrap();

            sut.Add(1, "1");

            DuplicateKeyException exception = Assert.Throws<DuplicateKeyException>(() => sut.Add(1, "2"));

            Assert.Equal(1, exception.DuplicateKey);

            exception = Assert.Throws<DuplicateKeyException>(() => sut.Add(new KeyValuePair<int, string>(1, "3")));

            Assert.Equal(1, exception.DuplicateKey);
        }

        [Fact]
        public void Should_Not_Throw_Totally_Useless_Exception_On_Key_Not_Found()
        {
            IDictionary<int, string> sut = new Dictionary<int, string>().Wrap();

            var exception = Assert.Throws<KeyNotFoundException>(() => sut[1]);

            Assert.Equal(1, exception.MissingKey);
        }
    }

    public static class DictionaryExtensions
    {
        public static IDictionary<TKey, TValue> Wrap<TKey, TValue>(this IDictionary<TKey, TValue> dict)
        {
            Contract.Requires(dict != null);
            Contract.Ensures(Contract.Result<IDictionary<TKey, TValue>>() != null);

            return new DictionaryWrapper<TKey, TValue>(dict);
        }
    }

    public class DuplicateKeyException : ArgumentException
    {
        private readonly object duplicateKey;

        public DuplicateKeyException(string paramName, Exception innerException, object duplicateKey)
            : base(string.Format(Infrastructure.Properties.Resources.DuplicateKeyException, duplicateKey), paramName, innerException)
        {
            this.duplicateKey = duplicateKey;
        }

        public object DuplicateKey
        {
            get
            {
                return this.duplicateKey;
            }
        }
    }

    public class KeyNotFoundException : System.Collections.Generic.KeyNotFoundException
    {
        private readonly object missingKey;

        public KeyNotFoundException(System.Collections.Generic.KeyNotFoundException innerException, object missingKey)
            : base(string.Format(Infrastructure.Properties.Resources.KeyNotFoundException, missingKey), innerException)
        {
            this.missingKey = missingKey;
        }

        public object MissingKey
        {
            get
            {
                return this.missingKey;
            }
        }
    }

    public class DictionaryWrapper<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> target;

        public DictionaryWrapper(IDictionary<TKey, TValue> target)
        {
            Contract.Requires(target != null);

            this.target = target;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.target.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
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

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            this.target.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return this.target.Remove(item);
        }

        public int Count
        {
            get
            {
                return this.target.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return this.target.IsReadOnly;
            }
        }

        public bool ContainsKey(TKey key)
        {
            return this.target.ContainsKey(key);
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

        public bool Remove(TKey key)
        {
            return this.target.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.target.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get
            {
                try
                {
                    return this.target[key];
                }
                catch (System.Collections.Generic.KeyNotFoundException ex)
                {
                    throw new KeyNotFoundException(ex, key);
                }
            }
            set
            {
                this.target[key] = value;
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                return this.target.Keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                return this.target.Values;
            }
        }
    }
}
