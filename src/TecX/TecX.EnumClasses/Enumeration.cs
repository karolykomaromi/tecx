namespace TecX.EnumClasses
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

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
            this.value = value;
            this.displayName = displayName;
            this.name = name;
        }

        public int Value
        {
            get { return this.value; }
        }

        public string DisplayName
        {
            get { return this.displayName; }
        }

        public string Name
        {
            get { return this.name; }
        }

        public static T FromValue<T>(int value) where T : Enumeration, new()
        {
            var matchingItem = Parse<T, int>(value, "value", item => item.Value == value);

            return matchingItem;
        }

        public static T FromDisplayName<T>(string displayName) where T : Enumeration, new()
        {
            var matchingItem = Parse<T, string>(displayName, "display displayName", item => string.Equals(item.DisplayName, displayName, StringComparison.OrdinalIgnoreCase));

            return matchingItem;
        }

        public static T FromName<T>(string name) where T : Enumeration, new()
        {
            var matchingItem = Parse<T, string>(name, "displayName", item => string.Equals(item.Name, name, StringComparison.OrdinalIgnoreCase));

            return matchingItem;
        }

        public static IEnumerable<T> GetAll<T>() where T : Enumeration, new()
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

        public static IEnumerable GetAll(Type type)
        {
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

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

        private static TEnumeration Parse<TEnumeration, TValue>(TValue value, string description, Func<TEnumeration, bool> predicate) where TEnumeration : Enumeration, new()
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
