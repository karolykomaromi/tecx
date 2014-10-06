namespace TecX.Unity.Utility
{
    using System.Collections.Generic;

    using TecX.Common;

    /// <summary>
    /// Extension methods for <see cref="IList{T}"/>
    /// </summary>
    public static class ListExtensions
    {
        public static void Fill<T>(this IList<T> list, T value)
        {
            Guard.AssertNotNull(list, "list");

            if (list.Contains(value))
            {
                return;
            }

            list.Add(value);
        }
    }
}