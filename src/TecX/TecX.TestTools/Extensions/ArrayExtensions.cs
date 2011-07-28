using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TecX.Common;

namespace TecX.TestTools.Extensions
{
    public static class ArrayExtensions
    {
        public static int IndexOf<T>(this T[] array, T item)
        {
            Guard.AssertNotNull(array, "array");
            Guard.AssertNotNull(item, "item");

            for(int i = 0; i < array.Length; i++)
            {
                if(array[i] != null)
                {
                    if (array[i].Equals(item))
                        return i;
                }
            }

            return -1;
        }
    }
}
