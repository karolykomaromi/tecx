namespace TecX.Common.Test.Collections
{
    using System.Collections.Generic;

    using TecX.Common.Collections;

    using Xunit;

    public class IndexedReadonlyCollectionFixture
    {
        [Fact]
        public void Should_Enumerate_Inner_Collection()
        {
            var inner = new List<int> { 1, 2, 3 };

            IIndexedReadOnlyCollection<int> collection = new IndexedReadOnlyCollection<int>(new List<int>(inner));

            Assert.Equal(inner, collection);
        }

        [Fact]
        public void Should_UseSameIndex()
        {
            var inner = new List<int> { 1, 2, 3 };

            IIndexedReadOnlyCollection<int> collection = new IndexedReadOnlyCollection<int>(inner);

            Assert.Equal(inner[0], collection[0]);
            Assert.Equal(inner[1], collection[1]);
            Assert.Equal(inner[2], collection[2]);
        }
    }
}
