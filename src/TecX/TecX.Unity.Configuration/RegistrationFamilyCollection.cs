using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TecX.Unity.Configuration
{
    /// <summary>
    /// Custom collection class for PluginFamily's
    /// </summary>
    public class RegistrationFamilyCollection : IEnumerable<RegistrationFamily>
    {
        private readonly Dictionary<Type, RegistrationFamily> _registrationFamilies;

        public RegistrationFamilyCollection()
        {
            _registrationFamilies = new Dictionary<Type, RegistrationFamily>();
        }

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

        public int Count { get { return _registrationFamilies.Count; } }

        public IEnumerable<RegistrationFamily> All
        {
            get
            {
                var families = new RegistrationFamily[_registrationFamilies.Count];

                _registrationFamilies.Values.CopyTo(families, 0);

                return families;
            }
        }

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
            _registrationFamilies.Remove(family.From);
        }

        public bool Contains(Type from)
        {
            return _registrationFamilies.ContainsKey(from);
        }

        public bool Contains<T>()
        {
            return Contains(typeof(T));
        }

        public void Each(Action<RegistrationFamily> action)
        {
            foreach (RegistrationFamily family in All)
            {
                action(family);
            }
        }
    }
}
