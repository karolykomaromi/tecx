namespace TecX.Unity.Configuration
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class RegistrationFamilyCollection : IEnumerable<RegistrationFamily>, IContainerConfigurator
    {
        private readonly Dictionary<Type, RegistrationFamily> registrationFamilies;

        public RegistrationFamilyCollection()
        {
            this.registrationFamilies = new Dictionary<Type, RegistrationFamily>();
        }

        public IEnumerable<RegistrationFamily> All
        {
            get
            {
                RegistrationFamily[] families = new RegistrationFamily[this.registrationFamilies.Count];

                this.registrationFamilies.Values.CopyTo(families, 0);

                return families;
            }
        }

        public int Count
        {
            get { return this.registrationFamilies.Count; }
        }

        public RegistrationFamily this[Type from]
        {
            get
            {
                RegistrationFamily family;
                if (!this.registrationFamilies.TryGetValue(from, out family))
                {
                    family = this.Add(new RegistrationFamily(from));
                }

                return family;
            }
        }

        IEnumerator<RegistrationFamily> IEnumerable<RegistrationFamily>.GetEnumerator()
        {
            return this.registrationFamilies.Values.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable<RegistrationFamily>)this).GetEnumerator();
        }

        public RegistrationFamily Add(RegistrationFamily family)
        {
            Guard.AssertNotNull(family, "family");
            Guard.AssertNotNull(family.From, "family.From");

            this.registrationFamilies[family.From] = family;

            return family;
        }

        public bool Contains(Type from)
        {
            Guard.AssertNotNull(from, "from");

            return this.registrationFamilies.ContainsKey(from);
        }

        public bool Contains<T>()
        {
            return this.Contains(typeof(T));
        }

        public void Each(Action<RegistrationFamily> action)
        {
            Guard.AssertNotNull(action, "action");

            foreach (RegistrationFamily family in this.All)
            {
                action(family);
            }
        }

        public void Configure(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            foreach (RegistrationFamily family in this.registrationFamilies.Values)
            {
                family.Configure(container);
            }
        }
    }
}
