namespace TecX.Unity.Utility
{
    using System;
    using System.Collections.Generic;

    using TecX.Common;

    /// <summary>
    /// Helper class for common operations on types
    /// </summary>
    public static class TypeHelper
    {
        public static bool IsInNamespace(this Type type, string nameSpace)
        {
            Guard.AssertNotNull(type, "type");
            Guard.AssertNotEmpty(nameSpace, "nameSpace");

            return type.Namespace.StartsWith(nameSpace);
        }

        public static IEnumerable<Type> AllInterfaces(this Type type)
        {
            foreach (Type @interface in type.GetInterfaces())
            {
                yield return @interface;
            }
        }

        public static bool IsConcrete(this Type type)
        {
            Guard.AssertNotNull(type, "type");

            return !type.IsAbstract && !type.IsInterface;
        }

        public static bool CanBeCreated(this Type type)
        {
            Guard.AssertNotNull(type, "type");

            return type.IsConcrete() && Constructor.HasConstructors(type);
        }

        public static bool CanBeCastTo(this Type pluggedType, Type pluginType)
        {
            if (pluggedType == null)
            {
                return false;
            }

            if (pluggedType.IsInterface || pluggedType.IsAbstract)
            {
                return false;
            }

            if (pluginType.IsOpenGeneric())
            {
                return GenericsHelper.CanBeCast(pluginType, pluggedType);
            }

            if (IsOpenGeneric(pluggedType))
            {
                return false;
            }

            return pluginType.IsAssignableFrom(pluggedType);
        }

        public static bool IsOpenGeneric(this Type type)
        {
            Guard.AssertNotNull(type, "type");

            return type.IsGenericTypeDefinition || type.ContainsGenericParameters;
        }

        public static IEnumerable<Type> GetAllBaseClassesAndInterfaces(this Type type)
        {
            Guard.AssertNotNull(type, "type");

            List<Type> allTypes = new List<Type>();

            Type current = type;
            while (current != null && current != typeof(object))
            {
                allTypes.Add(current);
                current = current.BaseType;
            }

            allTypes.AddRange(type.AllInterfaces());
            return allTypes;
        }
    }
}