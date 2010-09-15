using System;
using System.Collections.Generic;
using System.Linq;

namespace TecX.Common.Extensions.Primitives
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> GetBaseTypes(this Type type)
        {
            Guard.AssertNotNull(type, "type");

            if (type.BaseType == null)
            {
                return new[] { type };
            }

            return GetBaseTypes(type.BaseType)
                .Union(new[] { type });
        }
    }
}