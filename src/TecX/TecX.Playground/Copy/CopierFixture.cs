using System;
using System.Linq;

using Xunit;

namespace TecX.Playground.Copy
{
    public class CopierFixture
    {
        [Fact]
        public void CanCopyAllHierarchyLevels()
        {
            C original = new C { Foo = "Foo", Bar = -1, Baz = "Baz" };

            Copier copier = new Copier(CopyStrategies.All);

            C copy = copier.Copy(original);

            Assert.Equal(original.Foo, copy.Foo);
            Assert.Equal(original.Bar, copy.Bar);
            Assert.Equal(original.Baz, copy.Baz);
        }

        [Fact]
        public void CanSkipHierarchyLevels()
        {
            C original = new C { Foo = "Foo", Bar = -1, Baz = "Baz" };

            Copier copier = new Copier(CopyStrategies.All.Where(strategy => !string.Equals(strategy.GetType().Name, "BCopyStrategy", StringComparison.OrdinalIgnoreCase)));

            C copy = copier.Copy(original);

            Assert.Equal(original.Foo, copy.Foo);
            Assert.Equal(default(int), copy.Bar);
            Assert.Equal(original.Baz, copy.Baz);
        }
    }
}