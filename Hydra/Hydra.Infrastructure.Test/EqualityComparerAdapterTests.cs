namespace Hydra.Infrastructure.Test
{
    using System;
    using System.Collections;
    using Xunit;

    public class EqualityComparerAdapterTests
    {
        [Fact]
        public void Should_Equal()
        {
            IEqualityComparer sut = new EqualityComparerAdapter<string>(StringComparer.Ordinal);

            Assert.True(sut.Equals("a", "a"));
        }

        [Fact]
        public void Should_Not_Equal_On_Wrong_Type()
        {
            IEqualityComparer sut = new EqualityComparerAdapter<string>(StringComparer.Ordinal);

            Assert.False(sut.Equals("a", 1));
            Assert.False(sut.Equals(1, "a"));
        }

        [Fact]
        public void Should_GetHashCode()
        {
            IEqualityComparer sut = new EqualityComparerAdapter<string>(StringComparer.Ordinal);

            Assert.Equal(StringComparer.Ordinal.GetHashCode("a"), sut.GetHashCode("a"));
        }
    }
}