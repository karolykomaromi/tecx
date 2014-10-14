namespace Hydra.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Caching;
    using Hydra.Infrastructure.I18n;
    using Hydra.Infrastructure.Logging;

    public class Enumeration
    {
        private static IResourceAccessorCache resourceAccessorCache;

        static Enumeration()
        {
            Enumeration.resourceAccessorCache = new NullResourceAccessorCache();
        }

        public static void SetResourceAccessorCache(IResourceAccessorCache cache)
        {
            Contract.Requires(cache != null);

            Enumeration.resourceAccessorCache = cache;
        }

        internal static string ToString(IEnumeration enumeration)
        {
            Contract.Requires(enumeration != null);
            Contract.Ensures(Contract.Result<string>() != null);

            Func<string> accessor = Enumeration.resourceAccessorCache.GetAccessor(enumeration.GetType(), enumeration.Name);

            if (accessor != ResourceAccessorCache.EmptyAccessor)
            {
                return accessor();
            }

            return StringHelper.SplitCamelCase(enumeration.Name);
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public abstract class Enumeration<T> : IComparable<T>, IEquatable<T>, IEnumeration where T : Enumeration<T>
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
                if (!Enumeration<T>.IsInitialized)
                {
                    throw new EnumerationInitializationFailedException();
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

            if (!Enumeration<T>.IsInitialized)
            {
                throw new EnumerationInitializationFailedException();
            }

            return new ReadOnlyCollection<T>(Enumeration<T>.EnumerationValues.Values);
        }

        public static IReadOnlyCollection<string> GetNames()
        {
            Contract.Ensures(Contract.Result<IReadOnlyCollection<string>>() != null);

            if (!Enumeration<T>.IsInitialized)
            {
                throw new EnumerationInitializationFailedException();
            }

            return new ReadOnlyCollection<string>(Enumeration<T>.EnumerationValues.Values.Select(v => v.Name).ToList());
        }

        public static explicit operator int(Enumeration<T> enumeration)
        {
            Contract.Requires(enumeration != null);

            return enumeration.Value;
        }

        public static bool IsDefined(string name)
        {
            return Enumeration<T>.GetNames().Contains(name, StringComparer.OrdinalIgnoreCase);
        }

        public static bool IsDefined(int value)
        {
            IEnumerable<int> definedValues = Enumeration<T>.GetValues().Select(v => v.Value);

            bool isDefined = definedValues.Contains(value);

            return isDefined;
        }

        public override string ToString()
        {
            return Enumeration.ToString(this);
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
                FieldInfo[] values = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static);

                foreach (FieldInfo field in values)
                {
                    field.GetValue(null);
                }

                if (values.Length == 0)
                {
                    string message = string.Format(Properties.Resources.EnumerationDoesNotDeclareAnyFields, typeof(T).AssemblyQualifiedName);

                    throw new InvalidEnumerationException(message);
                }
            }
            catch (InvalidEnumerationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                HydraEventSource.Log.Warning(ex);

                return false;
            }

            return true;
        }
    }
}