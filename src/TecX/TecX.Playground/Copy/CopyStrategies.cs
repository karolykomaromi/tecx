using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using TecX.Common;

namespace TecX.Playground.Copy
{
    public static class CopyStrategies
    {
        private static readonly Lazy<IEnumerable<ICopyStrategy>> all = new Lazy<IEnumerable<ICopyStrategy>>(GetAllCopyStrategies);

        private static readonly Lazy<IList<Type>> allCopyStrategyTypes = new Lazy<IList<Type>>(GetAllCopyStrategyTypes);

        private static IEnumerable<ICopyStrategy> GetAllCopyStrategies()
        {
            var strategyTypes = allCopyStrategyTypes.Value;

            return new StrategyFactory(new ReadOnlyCollection<Type>(strategyTypes));
        }

        private static IList<Type> GetAllCopyStrategyTypes()
        {
            // get all classes that implement ICopyStrategy<> and have a parameterless default ctor from
            // all currently loaded assemblies that are not dynamically generated
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(assembly => !assembly.IsDynamic);

            var strategyTypes = assemblies
                .SelectMany(assembly => assembly.GetExportedTypes())
                .Where(type => type.IsClass &&
                               type.GetInterfaces().Any(
                                   i => i.IsGenericType &&
                                        typeof(ICopyStrategy<>).IsAssignableFrom(i.GetGenericTypeDefinition()) &&
                                        type.GetConstructor(Type.EmptyTypes) != null));

            return strategyTypes.ToList();
        }

        public static IEnumerable<ICopyStrategy> All
        {
            get
            {
                return all.Value;
            }
        }

        private class StrategyFactory : IEnumerable<ICopyStrategy>
        {
            private readonly IReadonlyCollection<Type> strategyTypes;
            private readonly List<Func<ICopyStrategy>> strategyFactories;

            public StrategyFactory(IReadonlyCollection<Type> strategyTypes)
            {
                Guard.AssertNotNull(strategyTypes, "strategyTypes");

                this.strategyTypes = strategyTypes;
                this.strategyFactories = new List<Func<ICopyStrategy>>();
            }

            public IEnumerator<ICopyStrategy> GetEnumerator()
            {
                if (strategyFactories.Count == 0)
                {
                    foreach (Type type in strategyTypes)
                    {
                        ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);

                        NewExpression @new = Expression.New(ctor);

                        UnaryExpression cast = Expression.Convert(@new, typeof(ICopyStrategy));

                        Expression<Func<ICopyStrategy>> lambda = Expression.Lambda<Func<ICopyStrategy>>(cast);

                        Func<ICopyStrategy> factory = lambda.Compile();

                        this.strategyFactories.Add(factory);
                    }
                }

                foreach (Func<ICopyStrategy> factory in strategyFactories)
                {
                    yield return factory();
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}