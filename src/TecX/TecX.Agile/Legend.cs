using System;
using System.Collections;
using System.Collections.Generic;

using TecX.Common;
using TecX.Common.Extensions.Error;

namespace TecX.Agile
{
    public class Legend : ICloneable, IEquatable<Legend>, IEnumerable<Mapping>
    {
        #region Fields

        private readonly IDictionary<string, Mapping> _mappings;

        #endregion Fields

        ////////////////////////////////////////////////////////////

        #region Properties

        public int Count
        {
            get { return _mappings.Count; }
        }

        #endregion Properties

        ////////////////////////////////////////////////////////////

        #region Indexer

        public Mapping this[string name]
        {
            get
            {
                Guard.AssertNotEmpty(name, "name");

                Mapping mapping;
                if (_mappings.TryGetValue(name, out mapping))
                    return mapping;

                return null;
            }
        }

        #endregion Indexer

        ////////////////////////////////////////////////////////////

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="Legend"/> class
        /// </summary>
        public Legend()
        {
            _mappings = new Dictionary<string, Mapping>();
        }

        #endregion c'tor

        ////////////////////////////////////////////////////////////

        #region Methods

        public void Clear()
        {
            _mappings.Clear();
        }

        public bool TryGetValue(string name, out Mapping mapping)
        {
            if (string.IsNullOrEmpty(name))
            {
                mapping = null;
                return false;
            }

            if (_mappings.TryGetValue(name, out mapping))
                return true;

            mapping = null;
            return false;
        }

        public void Add(string name, byte[] color)
        {
            Guard.AssertNotEmpty(name, "name");
            Guard.AssertNotNull(color, "color");

            if (color.Length != 4)
            {
                throw new ArgumentOutOfRangeException("color", "A color must be an array of four bytes (ARGB)");
            }

            if (_mappings.ContainsKey(name))
            {
                throw new ArgumentException("A mapping with the same name already exists", "name")
                    .WithAdditionalInfo("existing", _mappings[name]);
            }

            _mappings[name] = new Mapping(name, color);
        }

        public bool Remove(string name)
        {
            Guard.AssertNotEmpty(name, "name");

            return _mappings.Remove(name);
        }

        protected internal void CopyValueFrom(Legend other)
        {
            Guard.AssertNotNull(other, "other");

            _mappings.Clear();

            foreach (Mapping mapping in other)
            {
                Add(mapping.Name, (byte[]) mapping.Color.Clone());
            }
        }

        public Legend With(string name, byte[] color)
        {
            Add(name, color);

            return this;
        }

        #endregion Methods

        ////////////////////////////////////////////////////////////

        #region Implementation of ICloneable

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public object Clone()
        {
            Legend clone = new Legend();

            foreach (Mapping mapping in this)
            {
                clone.Add(mapping.Name, (byte[])mapping.Color.Clone());
            }

            return clone;
        }

        #endregion Implementation of ICloneable

        ////////////////////////////////////////////////////////////

        #region Implementation of IEquatable<Legend>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Legend other)
        {
            Guard.AssertNotNull(other, "other");

            if (Count != other.Count)
                return false;

            foreach (Mapping mapping in other)
            {
                Mapping m;
                if (_mappings.TryGetValue(mapping.Name, out m))
                {
                    if (!mapping.Equals(m))
                        return false;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            Guard.AssertNotNull(obj, "obj");

            Legend other = obj as Legend;

            if (other != null)
                return Equals(other);

            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        ////////////////////////////////////////////////////////////

        #region Implementation of IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<Mapping> GetEnumerator()
        {
            return _mappings.Values.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion Implementation of IEnumerable
    }
}