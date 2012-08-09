namespace TecX.Agile.Phone
{
    using System;
    using System.Collections.Generic;

    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            Guard.AssertNotNull(enumerable, "enumerable");
            Guard.AssertNotNull(action, "action");

            foreach(T item in enumerable)
            {
                action(item);
            }
        }
    }
}
