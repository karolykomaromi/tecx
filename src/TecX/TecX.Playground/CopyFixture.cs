namespace TecX.Playground
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Xunit;

    public class CopyFixture
    {
        [Fact]
        public void CanCopyAllHierarchyLevels()
        {
            C original = new C { Foo = "Foo", Bar = -1, Baz = "Baz" };

            var copyMaker = new CopyMaker(CopyStrategies.All);

            C copy = copyMaker.Copy(original);

            Assert.Equal(original.Foo, copy.Foo);
            Assert.Equal(original.Bar, copy.Bar);
            Assert.Equal(original.Baz, copy.Baz);
        }
    }

    public static class CopyStrategies
    {
        private static readonly Lazy<IEnumerable<ICopyStrategy>> all = new Lazy<IEnumerable<ICopyStrategy>>(GetAllCopyStrategies);

        private static IEnumerable<ICopyStrategy> GetAllCopyStrategies()
        {
            var types =
                AppDomain.CurrentDomain.GetAssemblies()
                         .Where(assembly => !assembly.IsDynamic)
                         .SelectMany(assembly => assembly.GetExportedTypes())
                         .Where(type => type.IsClass && typeof(ICopyStrategy).IsAssignableFrom(type));

            return types.Select(type => (ICopyStrategy)Activator.CreateInstance(type));
        }

        public static IEnumerable<ICopyStrategy> All
        {
            get
            {
                return all.Value;
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

    public class CopyMaker
    {
        private readonly IEnumerable<ICopyStrategy> copyStrategies;

        public CopyMaker(IEnumerable<ICopyStrategy> copyStrategies)
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
        private readonly CopyMaker copyMaker;

        public CopyContext(CopyMaker copyMaker)
        {
            this.copyMaker = copyMaker;
        }

        public CopyMaker CopyMaker
        {
            get
            {
                return this.copyMaker;
            }
        }
    }

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
