namespace TecX.Common.Extensions.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumerableExtensions
    {
        public static bool None<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            Guard.AssertNotNull(enumerable, "enumerable");
            Guard.AssertNotNull(predicate, "predicate");

            return !enumerable.Any(predicate);
        }
    }
}
