using System;
using System.Collections.Generic;
using System.Reflection;
using Search.Entities;

namespace Search
{
    public static class KnownTypes
    {
        public static IEnumerable<Type> AllEntities(ICustomAttributeProvider provider)
        {
            yield return typeof(SearchResult);
        }
    }
}