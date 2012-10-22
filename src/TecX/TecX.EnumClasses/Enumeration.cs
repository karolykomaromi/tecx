namespace TecX.EnumClasses
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using TecX.Common;

    [Serializable]
    public abstract class Enumeration : IComparable
    {
        private readonly int value;
        private readonly string displayName;
        private readonly string name;

        protected Enumeration()
        {
        }

        protected Enumeration(int value, string displayName, string name)
        {
            Guard.AssertIsInRange(value, "value", 0, int.MaxValue);
            Guard.AssertNotEmpty(displayName, "displayName");
            Guard.AssertNotEmpty(name, "name");

            this.value = value;
            this.displayName = displayName;
            this.name = name;
        }

        public virtual int Value
        {
            get { return this.value; }
        }

        public virtual string DisplayName
        {
            get { return this.displayName; }
        }

        public virtual string Name
        {
            get { return this.name; }
        }

        public static T FromValue<T>(int value) where T : Enumeration
        {
            Guard.AssertIsInRange(value, "value", 0, int.MaxValue);

            var matchingItem = Parse<T, int>(value, "value", item => item.Value == value);

            return matchingItem;
        }

        public static T FromDisplayName<T>(string displayName) where T : Enumeration
        {
            Guard.AssertNotEmpty(displayName, "displayName");

            var matchingItem = Parse<T, string>(displayName, "display name", item => string.Equals(item.DisplayName, displayName, StringComparison.OrdinalIgnoreCase));

            return matchingItem;
        }

        public static T FromName<T>(string name) where T : Enumeration
        {
            Guard.AssertNotEmpty(name, "name");

            var matchingItem = Parse<T, string>(name, "name", item => string.Equals(item.Name, name, StringComparison.OrdinalIgnoreCase));

            return matchingItem;
        }

        public static Enumeration FromValue(Type enumerationType, int value)
        {
            Guard.AssertNotNull(enumerationType, "enumerationType");
            Guard.AssertIsInRange(value, "value", 0, int.MaxValue);

            var matchingItem = Parse(enumerationType, value, "value", e => e.Value == value);

            return matchingItem;
        }

        public static Enumeration FromName(Type enumerationType, string name)
        {
            Guard.AssertNotNull(enumerationType, "enumerationType");
            Guard.AssertNotEmpty(name, "name");

            var matchingItem = Parse(enumerationType, name, "name", e => e.Name == name);

            return matchingItem;
        }

        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            var type = typeof(T);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            foreach (var info in fields)
            {
                var locatedValue = info.GetValue(null) as T;

                if (locatedValue != null)
                {
                    yield return locatedValue;
                }
            }
        }

        public static IEnumerable GetAll(Type enumerationType)
        {
            Guard.AssertNotNull(enumerationType, "enumerationType");

            var fields = enumerationType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            foreach (var info in fields)
            {
                yield return info.GetValue(null);
            }
        }

        public override string ToString()
        {
            return this.DisplayName;
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration;

            if (otherValue == null)
            {
                return false;
            }

            var typeMatches = this.GetType() == obj.GetType();
            var valueMatches = this.Value.Equals(otherValue.Value);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public virtual int CompareTo(object other)
        {
            return this.Value.CompareTo(((Enumeration)other).Value);
        }

        private static Enumeration Parse(Type enumerationType, object value, string description, Func<Enumeration, bool> predicate)
        {
            if (!typeof(Enumeration).IsAssignableFrom(enumerationType))
            {
                string message = string.Format("Type '{0}' is not an Enumeration", enumerationType);

                throw new EnumerationParseException(message);
            }

            var matchingItem = GetAll(enumerationType).OfType<Enumeration>().FirstOrDefault(predicate);

            if (matchingItem == null)
            {
                string message = string.Format("'{0}' is not a valid {1} in {2}", value, description, enumerationType);

                throw new EnumerationParseException(message);
            }

            return matchingItem;
        }

        private static TEnumeration Parse<TEnumeration, TValue>(TValue value, string description, Func<TEnumeration, bool> predicate) where TEnumeration : Enumeration
        {
            var matchingItem = GetAll<TEnumeration>().FirstOrDefault(predicate);

            if (matchingItem == null)
            {
                var message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof(TEnumeration));

                throw new ApplicationException(message);
            }

            return matchingItem;
        }
    }
}
