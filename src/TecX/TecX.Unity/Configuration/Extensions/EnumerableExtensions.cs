namespace TecX.Unity.Configuration.Extensions
{
    using System;
    using System.Collections.Generic;

    internal static class EnumerableExtensions
    {
        public static IEnumerable<T> Each<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T target in enumerable)
            {
                action(target);
            }

            return enumerable;
        }
    }
}
