namespace Hydra.Infrastructure.Test
{
    using System.Collections.Generic;
    using Xunit;

    public class EqualityComparerTests
    {
        [Fact]
        public void Should_Get_Correct_Default_Comparer()
        {
            EqualityComparerAdapter<int> actual = Assert.IsType<EqualityComparerAdapter<int>>(EqualityComparer.Default(typeof(int)));

            Assert.Same(EqualityComparer<int>.Default, actual.Comparer);
        }
    }
}