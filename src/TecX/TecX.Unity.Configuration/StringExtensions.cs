using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TecX.Unity.Configuration
{
    internal static class StringExtensions
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
