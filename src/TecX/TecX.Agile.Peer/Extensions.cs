using System;
using System.Collections.Generic;

using TecX.Common;

namespace TecX.Agile.Peer
{
    internal static class Extensions
    {
        public static int FindIndex<T>(this List<T> list, Predicate<T> match)
        {
            Guard.AssertNotNull(list, "list");
            Guard.AssertNotNull(match, "match");

            return list.FindIndex(0, match);
        }

        public static int FindIndex<T>(this List<T> list, int startIndex, Predicate<T> match)
        {
            Guard.AssertNotNull(list, "list");
            Guard.AssertNotNull(match, "match");

            return list.FindIndex(startIndex, list.Count - startIndex, match);
        }


        public static int FindIndex<T>(this List<T> list, int startIndex, int count, Predicate<T> match)
        {
            Guard.AssertNotNull(list, "list");
            Guard.AssertNotNull(match, "match");

            if (startIndex > list.Count)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }
            if ((count < 0) || (startIndex > (list.Count - count)))
            {
                throw new ArgumentOutOfRangeException("count");
            }
            
            int num = startIndex + count;
            for (int i = startIndex; i < num; i++)
            {
                if (match(list[i]))
                {
                    return i;
                }
            }
            return -1;

        }
    }

}
