using System;
using System.Collections.Generic;
using System.Linq;

using TecX.Common;
using TecX.Unity.Configuration.Common;

namespace TecX.Unity.Configuration.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> FindInterfacesThatClose(this Type pluggedType, Type templateType)
        {
            Guard.AssertNotNull(pluggedType, "pluggedType");
            Guard.AssertNotNull(templateType, "templateType");

            if (!pluggedType.IsConcrete()) yield break;

            if (templateType.IsInterface)
            {
                foreach (var interfaceType in pluggedType.GetInterfaces().Where(type => type.IsGenericType && (type.GetGenericTypeDefinition() == templateType)))
                {
                    yield return interfaceType;
                }
            }
            else if (pluggedType.BaseType.IsGenericType && (pluggedType.BaseType.GetGenericTypeDefinition() == templateType))
            {
                yield return pluggedType.BaseType;
            }

            if (pluggedType.BaseType == typeof(object)) yield break;

            foreach (var interfaceType in FindInterfacesThatClose(pluggedType.BaseType, templateType))
            {
                yield return interfaceType;
            }
        }
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

        public static void Fill<T>(this IList<T> list, T value)
        {
            Guard.AssertNotNull(list, "list");

            if (list.Contains(value))
                return;

            list.Add(value);
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

        /// <summary>
        /// Determines if the pluggedType can be upcast to the pluginType
        /// </summary>
        /// <param name="pluginType"></param>
        /// <param name="pluggedType"></param>
        /// <returns></returns>
        public static bool CanBeCastTo(this Type pluggedType, Type pluginType)
        {
            if (pluggedType == null) return false;

            if (pluggedType.IsInterface || pluggedType.IsAbstract)
            {
                return false;
            }

            if (pluginType.IsOpenGeneric())
            {
                return GenericsPluginGraph.CanBeCast(pluginType, pluggedType);
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
    }
}
