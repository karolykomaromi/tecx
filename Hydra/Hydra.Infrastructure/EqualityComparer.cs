namespace Hydra.Infrastructure
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Reflection;

    public class EqualityComparer
    {
        private static readonly ConcurrentDictionary<Type, IEqualityComparer> Comparers = new ConcurrentDictionary<Type, IEqualityComparer>();

        public static IEqualityComparer Default(Type type)
        {
            Contract.Requires(type != null);
            Contract.Ensures(Contract.Result<IEqualityComparer>() != null);

            IEqualityComparer comparer = Comparers.GetOrAdd(type, GetComparerForType);

            return comparer;
        }

        public static new bool Equals(object x, object y)
        {
            if (x == y)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            Type type = x.GetType();

            IEqualityComparer comparer = EqualityComparer.Default(type);

            return comparer.Equals(x, y);
        }

        private static IEqualityComparer GetComparerForType(Type type)
        {
            Type genericEqualityComparer = typeof(EqualityComparer<>).MakeGenericType(type);

            PropertyInfo @defaultProperty = genericEqualityComparer.GetProperty("Default", BindingFlags.Public | BindingFlags.Static);

            object c = @defaultProperty.GetValue(null);

            IEqualityComparer comparer = (IEqualityComparer)Activator.CreateInstance(typeof(EqualityComparerAdapter<>).MakeGenericType(type), c);

            return comparer;
        }
    }
}