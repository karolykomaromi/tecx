using System;
using System.Linq;
using System.Reflection;

namespace TecX.Unity.AutoRegistration
{
    public static class Filters
    {
        public static class ForTypes
        {
            public static Filter<Type> Is<TClass>()
                where TClass : class
            {
                Type type = typeof(TClass);

                return new Filter<Type>(t => t == type,
                    string.Format("Filter by concrete class '{0}'",
                    typeof(TClass).FullName));
            }

            public static Filter<Type> Implements<TContract>()
                where TContract : class
            {
                Type contract = typeof(TContract);

                if (!contract.IsInterface) throw new ArgumentException("Specified Type must be an interface!");

                return new Filter<Type>(t => t.GetInterfaces().Any(i => i == contract),
                    string.Format("Type implements contract '{0}'",
                    contract.FullName));
            }

            public static Filter<Type> IsDecoratedWith<TAttribute>()
                where TAttribute : Attribute
            {
                return new Filter<Type>(t => t.GetCustomAttributes(typeof(TAttribute), true).Any(),
                    string.Format("Type is decorated with attribute '{0}'",
                    typeof(TAttribute).FullName));
            }

            public static Filter<Type> ImplementsOpenGeneric(Type contract)
            {
                if (contract == null) throw new ArgumentNullException("contract");
                if (!contract.IsGenericTypeDefinition)
                    throw new ArgumentException("Provided contract has to be an open generic", "contract");

                return new Filter<Type>(t => t.GetInterfaces().Any(i => i.IsGenericType && (i.GetGenericTypeDefinition() == contract)),
                    string.Format("Type implements open generic '{0}'", contract.FullName));
            }

            public static Filter<Type> ImplementsITypeName()
            {
                return new Filter<Type>(t => t.GetInterfaces().Any(i => i.Name.StartsWith("I") && 
                    i.Name.Substring(1) == t.Name),
                    "Type implements an interface which has same name expect 'I' prefix.");
            }

            public static Filter<Type> ImplementsSingleInterface()
            {
                return new Filter<Type>(t => t.GetInterfaces().Count() == 1, "Type implements a single interface");
            }

            public static Filter<Type> IsAssignableFrom(Type type)
            {
                if (type == null) throw new ArgumentNullException("type");

                return new Filter<Type>(currentType => currentType.IsAssignableFrom(type),
                    string.Format("Type is assignable from '{0}'", type.FullName));
            }
        }

        public static class ForAssemblies
        {
            public static Filter<Assembly> IsSystemAssembly()
            {
                Filter<Assembly> filter = new Filter<Assembly>(a =>
                    a.GetName().Name == "mscorlib" ||
                    a.GetName().Name.StartsWith("System", StringComparison.OrdinalIgnoreCase),
                    "Exclude system assemblies.");

                return filter;
            }

            public static Filter<Assembly> ContainsType<T>()
            {
                return new Filter<Assembly>(a => a == typeof(T).Assembly,
                    string.Format("Assembly contains Type '{0}'", typeof(T).FullName));
            }

            public static Filter<Assembly> NameContains(string namePart)
            {
                if (namePart == null) throw new ArgumentNullException("namePart");

                return new Filter<Assembly>(a => a.GetName().Name.Contains(namePart),
                    string.Format("Assembly name contains '{0}'", namePart));
            }
        }
    }
}
