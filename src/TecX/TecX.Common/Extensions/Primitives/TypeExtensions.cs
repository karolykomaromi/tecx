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

            var baseTypes = GetBaseTypesInternal(type);

            baseTypes = baseTypes.Except(new[] {type});

            return baseTypes;
        }

        private static IEnumerable<Type> GetBaseTypesInternal(Type type)
        {
            if (type.BaseType == null)
            {
                return new[] { type };
            }

            return GetBaseTypesInternal(type.BaseType)
                .Union(new[] { type });
        }
    }
}