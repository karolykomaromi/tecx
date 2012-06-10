namespace TecX.Agile.Modules.Gestures
{
    using System.Collections;

    using TecX.Common;

    internal static class CollectionExtensions
    {
        public static bool IsEmpty(this ICollection collection)
        {
            Guard.AssertNotNull(collection, "collection");

            return collection.Count == 0;
        }
    }
}
