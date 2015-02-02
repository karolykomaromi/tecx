namespace Search
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Search.Entities;

    public static class KnownTypes
    {
        public static IEnumerable<Type> AllEntities(ICustomAttributeProvider provider)
        {
            yield return typeof(SearchResult);
        }
    }
}