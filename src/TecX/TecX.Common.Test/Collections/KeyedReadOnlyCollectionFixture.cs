namespace TecX.Common.Test.Collections
{
    using System.Collections.Generic;

    using TecX.Common.Collections;

    using Xunit;

    public class KeyedReadOnlyCollectionFixture
    {
        [Fact]
        public void Should_EnumerateAllValues()
        {
            var inner = new Dictionary<int, string> { { 1, "1" }, { 2, "2" }, { 3, "3" } };

            IKeyedReadOnlyCollections<int, string> collection = new KeyedReadOnlyCollection<int, string>(new Dictionary<int, string>(inner));

            Assert.Equal(inner.Values, collection);
        }

        [Fact]
        public void Should_UseSameKeys()
        {
            var inner = new Dictionary<int, string> { { 1, "1" }, { 2, "2" }, { 3, "3" } };

            IKeyedReadOnlyCollections<int, string> collection = new KeyedReadOnlyCollection<int, string>(inner);

            string value;
            Assert.True(collection.TryGetValue(1, out value));
            Assert.Equal("1", value);
            
            Assert.True(collection.TryGetValue(2, out value));
            Assert.Equal("2", value);
            
            Assert.True(collection.TryGetValue(3, out value));
            Assert.Equal("3", value);
        }
    }
}