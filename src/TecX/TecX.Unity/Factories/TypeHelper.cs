namespace TecX.Unity.Factories
{
    using System;
    using System.Collections.Generic;

    using TecX.Common;

    internal static class TypeHelper
    {
        public static bool TryGetCompatibleCollectionItemType(this Type type, out Type itemType)
        {
            Guard.AssertNotNull(type, "type");

            if (type.IsArray)
            {
                itemType = type.GetElementType();
                return true;
            }

            if (type.IsGenericType == false ||
                type.IsGenericTypeDefinition)
            {
                itemType = null;
                return false;
            }

            var openGeneric = type.GetGenericTypeDefinition();

            if (openGeneric == typeof(IList<>) ||
                openGeneric == typeof(ICollection<>) ||
                openGeneric == typeof(IEnumerable<>))
            {
                itemType = type.GetGenericArguments()[0];
                return true;
            }

            itemType = null;
            return false;
        }
    }
}