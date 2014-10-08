namespace Hydra.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;

    public abstract class Enumeration<T> : IComparable<T>, IEquatable<T> where T : Enumeration<T>
    {
        protected const int CompositeSortKey = int.MaxValue;

        protected static readonly SortedList<int, T> EnumerationValues = new SortedList<int, T>();

        private static readonly Lazy<bool> IsInitializedField = new Lazy<bool>(Initialize);

        private readonly string name;

        private readonly int sortKey;

        protected Enumeration(string name, int sortKey)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Requires(sortKey >= 0);

            this.name = name;
            this.sortKey = sortKey;

            if (sortKey < CompositeSortKey)
            {
                Enumeration<T>.EnumerationValues.Add(this.SortKey, (T)this);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static bool IsInitialized
        {
            get
            {
                return IsInitializedField.Value;
            }
        }

        public static T Default
        {
            get
            {
                if (!IsInitialized)
                {
                    throw new EnumerationInitializationException();
                }

                return Enumeration<T>.EnumerationValues.Values.First();
            }
        }

        public virtual string Name
        {
            get
            {
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

                return this.name;
            }
        }

        public virtual int Value
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);

                return Enumeration<T>.EnumerationValues.IndexOfKey(this.SortKey);
            }
        }

        protected int SortKey
        {
            get
            {
                return this.sortKey;
            }
        }

        public static IReadOnlyCollection<T> GetValues()
        {
            Contract.Ensures(Contract.Result<IReadOnlyCollection<T>>() != null);

            if (!IsInitialized)
            {
                throw new EnumerationInitializationException();
            }

            return new ReadOnlyCollection<T>(Enumeration<T>.EnumerationValues.Values);
        }

        public static IReadOnlyCollection<string> GetNames()
        {
            Contract.Ensures(Contract.Result<IReadOnlyCollection<string>>() != null);

            if (!IsInitialized)
            {
                throw new EnumerationInitializationException();
            }

            return new ReadOnlyCollection<string>(Enumeration<T>.EnumerationValues.Values.Select(v => v.Name).ToList());
        }

        public static bool IsDefined(string name)
        {
            return GetNames().Contains(name, StringComparer.OrdinalIgnoreCase);
        }

        public static bool IsDefined(int value)
        {
            IEnumerable<int> definedValues = GetValues().Select(v => v.Value);

            bool isDefined = definedValues.Contains(value);

            return isDefined;
        }

        public override string ToString()
        {
            return this.Name;
        }

        public override int GetHashCode()
        {
            return this.Value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            T other = obj as T;

            if (other == null)
            {
                return false;
            }

            return this.Value == other.Value;
        }

        public int CompareTo(T other)
        {
            Contract.Ensures(other != null);

            return this.Value.CompareTo(other.Value);
        }

        public bool Equals(T other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Value == other.Value;
        }

        private static bool Initialize()
        {
            try
            {
                var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static);

                foreach (var field in fields)
                {
                    field.GetValue(null);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}