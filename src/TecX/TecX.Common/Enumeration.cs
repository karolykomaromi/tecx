namespace TecX.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public abstract class Enumeration : IComparable
    {
        private readonly int value;

        private readonly string displayName;

        protected Enumeration()
        {
        }

        protected Enumeration(int value, string displayName)
        {
            Guard.AssertNotEmpty(displayName, "displayName");

            this.value = value;
            this.displayName = displayName;
        }

        public int Value
        {
            get { return this.value; }
        }

        public string DisplayName
        {
            get { return this.displayName; }
        }

        public static int AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
        {
            var absoluteDifference = Math.Abs(firstValue.Value - secondValue.Value);
            return absoluteDifference;
        }

        public static TEnum FromValue<TEnum>(int value) where TEnum : Enumeration, new()
        {
            var matchingItem = Parse<TEnum, int>(value, "value", item => item.Value == value);
            return matchingItem;
        }

        public static TEnum FromDisplayName<TEnum>(string displayName) where TEnum : Enumeration, new()
        {
            var matchingItem = Parse<TEnum, string>(displayName, "display name", item => item.DisplayName == displayName);

            return matchingItem;
        }

        public static IEnumerable<TEnum> GetAll<TEnum>() where TEnum : Enumeration, new()
        {
            var type = typeof(TEnum);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            foreach (var info in fields)
            {
                var instance = new TEnum();
                var locatedValue = info.GetValue(instance) as TEnum;

                if (locatedValue != null)
                {
                    yield return locatedValue;
                }
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
            var valueMatches = this.value.Equals(otherValue.Value);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }

        public int CompareTo(object other)
        {
            return this.Value.CompareTo(((Enumeration)other).Value);
        }

        private static TEnum Parse<TEnum, K>(K value, string description, Func<TEnum, bool> predicate)
            where TEnum : Enumeration, new()
        {
            var matchingItem = GetAll<TEnum>().FirstOrDefault(predicate);

            if (matchingItem == null)
            {
                var message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof(TEnum));
                throw new ApplicationException(message);
            }

            return matchingItem;
        }
    }
}
