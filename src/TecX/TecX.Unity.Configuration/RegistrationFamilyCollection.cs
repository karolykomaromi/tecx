using System;
using System.Collections;
using System.Collections.Generic;

using TecX.Common;

namespace TecX.Unity.Configuration
{
    public class RegistrationFamilyCollection : IEnumerable<RegistrationFamily>
    {
        #region Fields

        private readonly Dictionary<Type, RegistrationFamily> _registrationFamilies;

        #endregion Fields

        #region Properties

        public IEnumerable<RegistrationFamily> All
        {
            get
            {
                RegistrationFamily[] families = new RegistrationFamily[_registrationFamilies.Count];

                _registrationFamilies.Values.CopyTo(families, 0);

                return families;
            }
        }

        public int Count
        {
            get { return _registrationFamilies.Count; }
        }

        #endregion Properties

        #region Indexer

        public RegistrationFamily this[Type from]
        {
            get
            {
                if (!_registrationFamilies.ContainsKey(from))
                {
                    var family = new RegistrationFamily(from);
                    Add(family);
                }

                return _registrationFamilies[from];
            }
        }

        #endregion Indexer

        #region c'tor

        public RegistrationFamilyCollection()
        {
            _registrationFamilies = new Dictionary<Type, RegistrationFamily>();
        }

        #endregion c'tor

        #region IEnumerable<RegistrationFamily> Members

        IEnumerator<RegistrationFamily> IEnumerable<RegistrationFamily>.GetEnumerator()
        {
            return _registrationFamilies.Values.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable<RegistrationFamily>)this).GetEnumerator();
        }

        #endregion IEnumerable<RegistrationFamily> Members

        public RegistrationFamily Add(RegistrationFamily family)
        {
            Guard.AssertNotNull(family, "family");

            Type key = family.From;
            if (_registrationFamilies.ContainsKey(key))
            {
                _registrationFamilies[key] = family;
            }
            else
            {
                _registrationFamilies.Add(key, family);
            }

            return family;
        }

        public void Remove(RegistrationFamily family)
        {
            Guard.AssertNotNull(family, "family");

            _registrationFamilies.Remove(family.From);
        }

        public bool Contains(Type from)
        {
            Guard.AssertNotNull(from, "from");

            return _registrationFamilies.ContainsKey(from);
        }

        public bool Contains<T>()
        {
            return Contains(typeof(T));
        }

        public void Each(Action<RegistrationFamily> action)
        {
            Guard.AssertNotNull(action, "action");

            foreach (RegistrationFamily family in All)
            {
                action(family);
            }
        }
    }
}
