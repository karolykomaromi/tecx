namespace Hydra.Infrastructure.Test
{
    using System.Collections;
    using System.Collections.Generic;
    using Xunit;

    public class DefaultTests
    {
        [Fact]
        public void Default_For_String_Should_Be_Empty_String()
        {
            Assert.Equal(string.Empty, Default.Value<string>());
        }

        [Fact]
        public void Default_For_Array_Should_Be_Empty_Array()
        {
            Assert.IsType<int[]>(Default.Value<int[]>());
        }

        [Fact]
        public void Default_For_Generic_Collection_Interface_Should_Be_Empty_Array()
        {
            Assert.IsType<int[]>(Default.Value<IList<int>>());
            Assert.IsType<int[]>(Default.Value<ICollection<int>>());
            Assert.IsType<int[]>(Default.Value<IEnumerable<int>>());
        }

        [Fact]
        public void Default_For_Non_Generic_Collection_Interface_Should_Be_Empty_Array()
        {
            Assert.IsType<object[]>(Default.Value<IList>());
            Assert.IsType<object[]>(Default.Value<ICollection>());
            Assert.IsType<object[]>(Default.Value<IEnumerable>());
        }

        [Fact]
        public void Default_Should_Be_Empty_List()
        {
            Assert.IsType<List<int>>(Default.Value<List<int>>());
        }

        [Fact]
        public void Default_Should_Be_Empty_Dictionary()
        {
            Assert.IsType<Dictionary<string, long>>(Default.Value<Dictionary<string, long>>());
            Assert.IsType<Dictionary<string, long>>(Default.Value<IDictionary<string, long>>());
        }
    }
}