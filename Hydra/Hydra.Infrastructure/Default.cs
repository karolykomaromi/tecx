namespace Hydra.Infrastructure
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class Default
    {
        private static readonly ConcurrentDictionary<Type, Func<object>> DefaultFactories = new ConcurrentDictionary<Type, Func<object>>();

        public static T Value<T>()
        {
            Type type = typeof(T);

            return (T)DefaultFactories.GetOrAdd(type, CreateDefaultFactory)();
        }

        private static Func<object> CreateDefaultFactory(Type type)
        {
            if (type == typeof(string))
            {
                return () => string.Empty;
            }

            if (type.IsArray)
            {
                Type arrayType = type.GetElementType();

                return EmptyArray(arrayType);
            }

            if (IsGenericList(type))
            {
                return EmptyList(type);
            }

            if (IsGenericDictionaryOrInterface(type))
            {
                return EmptyDictionary(type);
            }

            if (IsGenericCollectionInterface(type))
            {
                Type genericArgument = type.GetGenericArguments()[0];

                return EmptyArray(genericArgument);
            }

            if (IsNonGenericCollectionInterface(type))
            {
                return EmptyArray(typeof(object));
            }

            return FrameworkDefault(type);
        }

        private static Func<object> EmptyDictionary(Type type)
        {
            Type[] genericArguments = type.GetGenericArguments();
            Type dictionaryType = typeof(Dictionary<,>).MakeGenericType(genericArguments[0], genericArguments[1]);

            ConstructorInfo ctor = dictionaryType.GetConstructor(Type.EmptyTypes);

            NewExpression @new = Expression.New(ctor);

            UnaryExpression cast = Expression.Convert(@new, typeof(object));

            var lambda = Expression.Lambda<Func<object>>(cast);

            Func<object> factory = lambda.Compile();

            return factory;
        }

        private static bool IsGenericDictionaryOrInterface(Type type)
        {
            return type.IsGenericType &&
                   (type.GetGenericTypeDefinition() == typeof(Dictionary<,>) ||
                    type.GetGenericTypeDefinition() == typeof(IDictionary<,>));
        }

        private static Func<object> EmptyList(Type type)
        {
            ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);

            NewExpression @new = Expression.New(ctor);

            UnaryExpression cast = Expression.Convert(@new, typeof(object));

            var lambda = Expression.Lambda<Func<object>>(cast);

            Func<object> factory = lambda.Compile();

            return factory;
        }

        private static Func<object> FrameworkDefault(Type type)
        {
            DefaultExpression @default = Expression.Default(type);

            UnaryExpression cast = Expression.Convert(@default, typeof(object));

            var lambda = Expression.Lambda<Func<object>>(cast);

            Func<object> factory = lambda.Compile();

            return factory;
        }

        private static Func<object> EmptyArray(Type genericArgument)
        {
            NewArrayExpression newArray = Expression.NewArrayInit(genericArgument);

            UnaryExpression cast = Expression.Convert(newArray, typeof(object));

            var lambda = Expression.Lambda<Func<object>>(cast);

            Func<object> factory = lambda.Compile();

            return factory;
        }

        private static bool IsGenericList(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }

        private static bool IsGenericCollectionInterface(Type type)
        {
            return type.IsGenericType &&
                   (type.GetGenericTypeDefinition() == typeof(IList<>) ||
                    type.GetGenericTypeDefinition() == typeof(ICollection<>) ||
                    type.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }

        private static bool IsNonGenericCollectionInterface(Type type)
        {
            return type == typeof(IList) ||
                   type == typeof(ICollection) ||
                   type == typeof(IEnumerable);
        }
    }
}