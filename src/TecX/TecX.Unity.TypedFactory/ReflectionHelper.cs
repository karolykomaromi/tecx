using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using TecX.Common;

namespace TecX.Unity.TypedFactory
{
    public static class ReflectionHelper
    {
        /// <summary>
        /// If the extended type is a Foo[] or IEnumerable{Foo} which is assignable from Foo[] this method will return typeof(Foo)
        /// otherwise <c>null</c>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetCompatibleArrayItemType(this Type type)
        {
            Guard.AssertNotNull(type, "type");

            if (type.IsArray)
            {
                return type.GetElementType();
            }

            if (type.IsGenericType == false ||
                type.IsGenericTypeDefinition)
            {
                return null;
            }

            var openGeneric = type.GetGenericTypeDefinition();

            if (openGeneric == typeof(IList<>) ||
                openGeneric == typeof(ICollection<>) ||
                openGeneric == typeof(IEnumerable<>))
            {
                return type.GetGenericArguments().Single();
            }

            return null;
        }

        public static Delegate CastItemAndAddToList(Type itemType)
        {
            Type listType = typeof(List<>).MakeGenericType(itemType);

            //find the Add(T) method of class List<T>
            MethodInfo addMethod = listType.GetMethod("Add");

            //define the parameters for the single item and the list to which it should be added
            ParameterExpression item = Expression.Parameter(typeof(object), "item");

            ParameterExpression list = Expression.Parameter(listType, "list");

            UnaryExpression castToItemType = Expression.Convert(item, itemType);

            MethodCallExpression add = Expression.Call(list, addMethod, new Expression[] { castToItemType });

            Delegate addCastedItemToList = Expression.Lambda(add, item, list).Compile();

            return addCastedItemToList;
        }

        public static object CreateGenericListOf(Type itemType)
        {
            Type listType = typeof(List<>).MakeGenericType(itemType);

            Type delegateType = typeof(Func<>).MakeGenericType(listType);
            
            NewExpression @new = Expression.New(listType);

            LambdaExpression lambda = Expression.Lambda(delegateType, @new);

            object list = lambda.Compile().DynamicInvoke();

            return list;
        }

        public static object ToArray(Type itemType, object list)
        {
            Type listType = typeof(List<>).MakeGenericType(itemType);

            ParameterExpression source = Expression.Parameter(listType, "list");

            var call = Expression.Call(typeof(Enumerable), "ToArray", new[] { itemType }, source);

            Type arrayType = itemType.MakeArrayType();

            var convert = Expression.Lambda(typeof(Func<,>).MakeGenericType(listType, arrayType), call, source).Compile();

            object converted = convert.DynamicInvoke(list);

            return converted;
        }
    }
}