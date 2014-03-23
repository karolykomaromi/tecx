namespace TecX.Unity.Factories
{
    using System;
    using System.Collections.Generic;

    using TecX.Common;

    internal static class TypeHelper
    {
        public static bool TryGetCompatibleCollectionItemType(Type collectionType, out Type itemType)
        {
            Guard.AssertNotNull(collectionType, "collectionType");

            if (collectionType.IsArray)
            {
                itemType = collectionType.GetElementType();
                return true;
            }

            if (!collectionType.IsGenericType || collectionType.IsGenericTypeDefinition)
            {
                itemType = null;
                return false;
            }

            Type openGeneric = collectionType.GetGenericTypeDefinition();

            if (typeof(IList<>).IsAssignableFrom(openGeneric) ||
                typeof(ICollection<>).IsAssignableFrom(openGeneric) ||
                typeof(IEnumerable<>).IsAssignableFrom(openGeneric))
            {
                itemType = collectionType.GetGenericArguments()[0];
                return true;
            }

            itemType = null;
            return false;
        }
    }
}