﻿using System.Collections.Generic;

namespace TecX.Common.Extensions.Collections
{
    /// <summary>
    /// Extension methods for <see cref="IList{T}"/>
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// The sorely missed method to add a range of items that inherit from the type of the items
        /// in the list.
        /// </summary>
        /// <typeparam name="TList">The type of the list items</typeparam>
        /// <typeparam name="TRange">The type of the items in the range to add</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="range">The range.</param>
        public static void AddRange<TList, TRange>(this IList<TList> list, IEnumerable<TRange> range)
            where TRange : TList
        {
            Guard.AssertNotNull(list, "list");
            Guard.AssertNotNull(range, "range");

            foreach (TRange item in range)
            {
                list.Add(item);
            }
        }
    }
}