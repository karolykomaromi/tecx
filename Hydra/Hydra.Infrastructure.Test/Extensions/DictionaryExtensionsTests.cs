namespace Hydra.Infrastructure.Test.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Hydra.Infrastructure.Extensions;
    using Xunit;

    public class DictionaryExtensionsTests
    {
        [Fact]
        public void Should_Merge_Matching_Keys()
        {
            var first = new Dictionary<int, int> { { 1, 1 } };
            var second = new Dictionary<int, int> { { 1, 2 } };

            var actual = first.Merge(second, g => g.Sum());

            Assert.Equal(actual[1], 3);
        }

        [Fact]
        public void Should_Merge_Matching_Keys_With_Custom_Comparer()
        {
            var first = new Dictionary<string, int> { { "a", 1 } };
            var second = new Dictionary<string, int> { { "A", 2 } };

            var actual = first.Merge(second, g => g.Sum(), StringComparer.OrdinalIgnoreCase);

            Assert.Equal(actual["a"], 3);
        }

        [Fact]
        public void Should_Merge_Non_Matching_Keys()
        {
            var first = new Dictionary<int, int> { { 1, 1 } };
            var second = new Dictionary<int, int> { { 2, 1 } };

            var actual = first.Merge(second, g => g.Sum());

            Assert.Equal(actual[1], 1);
            Assert.Equal(actual[2], 1);
        }

        [Fact]
        public void Should_Merge_Non_Matching_Keys_With_Custom_Comparer()
        {
            var first = new Dictionary<string, int> { { "a", 1 } };
            var second = new Dictionary<string, int> { { "b", 1 } };

            var actual = first.Merge(second, g => g.Sum(), StringComparer.OrdinalIgnoreCase);

            Assert.Equal(actual["A"], 1);
            Assert.Equal(actual["B"], 1);
        }


    }
}