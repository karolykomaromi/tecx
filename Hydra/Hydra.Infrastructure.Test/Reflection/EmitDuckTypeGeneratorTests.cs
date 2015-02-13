namespace Hydra.Infrastructure.Test.Reflection
{
    using System;
    using Hydra.Infrastructure.Reflection;
    using Xunit;

    public class EmitDuckTypeGeneratorTests
    {
        [Fact]
        public void Should_Treat_Structurally_Equivalent_Object_As_Duck()
        {
            IDuckTypeGenerator sut = new EmitDuckTypeGenerator();

            DuckButNot x = new DuckButNot
            {
                Foo = 1,
                Bar = "2"
            };

            IDuck actual = sut.Duck<IDuck>(x);

            Assert.NotNull(actual);
            Assert.Equal(1, actual.Foo);
            Assert.Equal("2", actual.Bar);

            actual.Baz(new object(), EventArgs.Empty);

            Assert.True(x.CalledBuz);

            Assert.Equal(x.TheAnswer(), actual.TheAnswer());
        }
    }
}
