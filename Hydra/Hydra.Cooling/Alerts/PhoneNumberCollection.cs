namespace Hydra.Cooling.Alerts
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    public class PhoneNumberCollection : IReadOnlyList<PhoneNumber>, IEquatable<PhoneNumberCollection>
    {
        private readonly List<PhoneNumber> phoneNumbers;

        public PhoneNumberCollection(params PhoneNumber[] phoneNumbers)
        {
            Contract.Requires(phoneNumbers != null);
            Contract.Requires(phoneNumbers.Length > 0);

            this.phoneNumbers = phoneNumbers
                .Distinct()
                .OrderBy(pn => pn, Comparer<PhoneNumber>.Default)
                .ToList();
        }

        public PhoneNumber this[int index]
        {
            get { return this.phoneNumbers[index]; }
        }

        public int Count
        {
            get { return this.phoneNumbers.Count; }
        }

        public IEnumerator<PhoneNumber> GetEnumerator()
        {
            return this.phoneNumbers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public override bool Equals(object obj)
        {
            PhoneNumberCollection other = obj as PhoneNumberCollection;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            int hashCode = this.phoneNumbers.Aggregate(0, (current, number) => current ^ number.GetHashCode());

            return hashCode;
        }

        public bool Equals(PhoneNumberCollection other)
        {
            if (other == null)
            {
                return false;
            }

            if (this.Count != other.Count)
            {
                return false;
            }

            return this.SequenceEqual(other);
        }
    }
}