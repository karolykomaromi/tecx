namespace Hydra.Infrastructure.Test
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using Xunit;

    public class DefaultTests
    {
        [Fact]
        public void Default_For_String_Should_Be_Empty_String()
        {
            Assert.Equal(string.Empty, Default.Value<string>());
        }

        [Fact]
        public void Default_For_Array_Should_Be_Empty_Array()
        {
            Assert.IsType<int[]>(Default.Value<int[]>());
        }

        [Fact]
        public void Default_For_Generic_Collection_Interface_Should_Be_Empty_Array()
        {
            Assert.IsType<int[]>(Default.Value<IList<int>>());
            Assert.IsType<int[]>(Default.Value<ICollection<int>>());
            Assert.IsType<int[]>(Default.Value<IEnumerable<int>>());
        }

        [Fact]
        public void Default_For_Non_Generic_Collection_Interface_Should_Be_Empty_Array()
        {
            Assert.IsType<object[]>(Default.Value<IList>());
            Assert.IsType<object[]>(Default.Value<ICollection>());
            Assert.IsType<object[]>(Default.Value<IEnumerable>());
        }

        [Fact]
        public void Default_Should_Be_Empty_List()
        {
            Assert.IsType<List<int>>(Default.Value<List<int>>());
        }
    }

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