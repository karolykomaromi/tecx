using System;
using System.Collections;
using System.Collections.Generic;

using TecX.Common;

namespace TecX.Unity.Configuration
{
    public class RegistrationFamilyCollection : IEnumerable<RegistrationFamily>
    {
        private readonly Dictionary<Type, RegistrationFamily> _registrationFamilies;

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

        public RegistrationFamily this[Type from]
        {
            get
            {
                RegistrationFamily family;
                if (!_registrationFamilies.TryGetValue(from, out family))
                {
                    family = Add(new RegistrationFamily(from));
                }

                return family;
            }
        }

        public RegistrationFamilyCollection()
        {
            _registrationFamilies = new Dictionary<Type, RegistrationFamily>();
        }

        IEnumerator<RegistrationFamily> IEnumerable<RegistrationFamily>.GetEnumerator()
        {
            return _registrationFamilies.Values.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable<RegistrationFamily>)this).GetEnumerator();
        }

        public RegistrationFamily Add(RegistrationFamily family)
        {
            Guard.AssertNotNull(family, "family");
            Guard.AssertNotNull(family.From, "family.From");

            _registrationFamilies[family.From] = family;

            return family;
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
