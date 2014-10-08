namespace TecX.TestTools.Extensions
{
    using TecX.Common;

    public static class ArrayExtensions
    {
        public static int IndexOf<T>(this T[] array, T item)
        {
            Guard.AssertNotNull(array, "array");
            Guard.AssertNotNull(item, "item");

            for (int i = 0; i < array.Length; i++)
            {
                if (object.Equals(array[i], item))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
