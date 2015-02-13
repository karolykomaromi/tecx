namespace Hydra.Unity.Test.Infrastructure.Reflection
{
    using System;
    using Hydra.Infrastructure.Reflection;
    using Hydra.Unity.Infrastructure.Reflection;
    using Xunit;

    public class UnityDuckTypeGeneratorTests
    {
        [Fact]
        public void Should_Treat_Anonymous_Object_As_Duck()
        {
            IDuckTypeGenerator sut = new UnityDuckTypeGenerator();

            bool called = false;

            var x = new
            {
                Foo = 1,
                Bar = "2",
                Baz = new Action<object, EventArgs>((s, e) => { called = true; }),
                TheAnswer = new Func<int>(() => 42)
            };

            IDuck actual = sut.Duck<IDuck>(x);

            Assert.NotNull(actual);
            Assert.Equal(1, actual.Foo);
            Assert.Equal<string>("2", actual.Bar);

            actual.Baz(new object(), EventArgs.Empty);

            Assert.True(called);

            Assert.Equal(x.TheAnswer(), actual.TheAnswer());
        }

        [Fact]
        public void Should_Treat_Structurally_Equivalent_Object_As_Duck()
        {
            IDuckTypeGenerator sut = new UnityDuckTypeGenerator();

            DuckButNot x = new DuckButNot
            {
                Foo = 1,
                Bar = "2"
            };

            IDuck actual = sut.Duck<IDuck>(x);

            Assert.NotNull(actual);
            Assert.Equal(1, actual.Foo);
            Assert.Equal<string>("2", actual.Bar);

            actual.Baz(new object(), EventArgs.Empty);

            Assert.True(x.CalledBuz);

            Assert.Equal(x.TheAnswer(), actual.TheAnswer());
        }

        [Fact]
        public void Should_Throw_On_Not_Implemented_Method()
        {
            IDuckTypeGenerator sut = new UnityDuckTypeGenerator();

            DuckButNot x = new DuckButNot();

            IDuck actual = sut.Duck<IDuck>(x);

            Assert.Throws<NotImplementedException>(() => actual.NotImplementedMethod());
        }

        [Fact]
        public void Should_Throw_On_Not_Implemented_Property()
        {
            IDuckTypeGenerator sut = new UnityDuckTypeGenerator();

            DuckButNot x = new DuckButNot();

            IDuck actual = sut.Duck<IDuck>(x);

            Assert.Throws<NotImplementedException>(() => actual.NotImplementedProperty);
        }
    }
}