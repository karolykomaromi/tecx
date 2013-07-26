
namespace TecX.Playground
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Collections;
    using System.Linq.Expressions;
    using System.Reflection;

    using TecX.Common;

    using Xunit;

    public class CopierFixture
    {
        [Fact]
        public void CanCopyAllHierarchyLevels()
        {
            C original = new C { Foo = "Foo", Bar = -1, Baz = "Baz" };

            var copyMaker = new Copier(CopyStrategies.All);

            C copy = copyMaker.Copy(original);

            Assert.Equal(original.Foo, copy.Foo);
            Assert.Equal(original.Bar, copy.Bar);
            Assert.Equal(original.Baz, copy.Baz);
        }

        [Fact]
        public void CanSkipHierarchyLevels()
        {
            C original = new C { Foo = "Foo", Bar = -1, Baz = "Baz" };

            var copyMaker = new Copier(CopyStrategies.All.Where(strategy => !string.Equals(strategy.GetType().Name, "BCopyStrategy", StringComparison.OrdinalIgnoreCase)));

            C copy = copyMaker.Copy(original);

            Assert.Equal(original.Foo, copy.Foo);
            Assert.Equal(default(int), copy.Bar);
            Assert.Equal(original.Baz, copy.Baz);
        }
    }

    public static class CopyStrategies
    {
        private static readonly Lazy<IEnumerable<ICopyStrategy>> all = new Lazy<IEnumerable<ICopyStrategy>>(GetAllCopyStrategies);

        private static IEnumerable<ICopyStrategy> GetAllCopyStrategies()
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

            return new StrategyFactory(strategyTypes.ToList());
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
            private readonly ICollection<Type> _StrategyTypes;
            private readonly List<Func<ICopyStrategy>> _StrategyFactories;

            public StrategyFactory(ICollection<Type> strategyTypes)
            {
                Guard.AssertNotEmpty(strategyTypes, "strategyTypes");

                _StrategyTypes = strategyTypes;
                _StrategyFactories = new List<Func<ICopyStrategy>>();
            }

            public IEnumerator<ICopyStrategy> GetEnumerator()
            {
                if (_StrategyFactories.Count == 0)
                {
                    foreach (Type type in _StrategyTypes)
                    {
                        ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);

                        NewExpression @new = Expression.New(ctor);

                        UnaryExpression cast = Expression.Convert(@new, typeof (ICopyStrategy));

                        Expression<Func<ICopyStrategy>> lambda = Expression.Lambda<Func<ICopyStrategy>>(cast);

                        Func<ICopyStrategy> factory = lambda.Compile();

                        this._StrategyFactories.Add(factory);
                    }
                }

                foreach (Func<ICopyStrategy> factory in _StrategyFactories)
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

    public class ACopyStrategy : ICopyStrategy<A>
    {
        public void CopyValues(A original, A copy, CopyContext ctx)
        {
            copy.Foo = original.Foo;
        }
    }

    public class BCopyStrategy : ICopyStrategy<B>
    {
        public void CopyValues(B original, B copy, CopyContext ctx)
        {
            copy.Bar = original.Bar;
        }
    }

    public class CCopyStrategy : ICopyStrategy<C>
    {
        public void CopyValues(C original, C copy, CopyContext ctx)
        {
            copy.Baz = original.Baz;
        }
    }

    public class Copier
    {
        private readonly IEnumerable<ICopyStrategy> copyStrategies;

        public Copier(IEnumerable<ICopyStrategy> copyStrategies)
        {
            this.copyStrategies = copyStrategies;
        }

        public T Copy<T>(T original) where T : A, new()
        {
            T copy = new T();

            CopyContext ctx = new CopyContext(this);

            var strategies = this.copyStrategies.OfType<ICopyStrategy<T>>();

            foreach (ICopyStrategy<T> strategy in strategies)
            {
                strategy.CopyValues(original, copy, ctx);
            }

            return copy;
        }
    }

    public class CopyContext
    {
        private readonly Copier _Copier;

        public CopyContext(Copier _Copier)
        {
            this._Copier = _Copier;
        }

        public Copier Copier
        {
            get
            {
                return this._Copier;
            }
        }
    }

    /// <summary>
    /// Marker interface
    /// </summary>
    public interface ICopyStrategy
    {
    }

    public interface ICopyStrategy<in T> : ICopyStrategy
        where T : A
    {
        void CopyValues(T original, T copy, CopyContext ctx);
    }

    public class C : B
    {
        public string Baz { get; set; }
    }

    public class B : A
    {
        public int Bar { get; set; }
    }

    public class A
    {
        public string Foo { get; set; }
    }
}
