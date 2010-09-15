using System;

namespace TecX.Unity.Registration
{
    public static class If
    {
        public static Filter<Type> Is<TClass>()
            where TClass : class
        {
            return Filters.ForTypes.Is<TClass>();
        }

        public static Filter<Type> Implements<TContract>()
            where TContract : class
        {
            return Filters.ForTypes.Implements<TContract>();
        }

        public static Filter<Type> DecoratedWith<TAttribute>()
            where TAttribute : Attribute
        {
            return Filters.ForTypes.IsDecoratedWith<TAttribute>();
        }

        public static Filter<Type> ImplementsOpenGeneric(Type openGeneric)
        {
            return Filters.ForTypes.ImplementsOpenGeneric(openGeneric);
        }

        public static Filter<Type> ImplementsITypeName()
        {
            return Filters.ForTypes.ImplementsITypeName();
        }

    }
}
