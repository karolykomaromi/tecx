namespace TecX.TestTools.Test.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using TecX.TestTools.Data;

    using Xunit;

    public class AppendOnlyCollectionFixture
    {
        [Fact]
        public void Should_EnumerateSameValues()
        {
            var inner = new List<int> { 1, 2, 3 };

            IAppendOnlyCollection<int> collection = new AppendOnlyCollection<int>(new List<int>(inner));

            Assert.Equal(inner, collection);
        }

        [Fact]
        public void Should_AddValue()
        {
            IAppendOnlyCollection<int> collection = new AppendOnlyCollection<int>();

            collection.Add(1);

            Assert.Contains(1, collection);
            Assert.Equal(1, collection.Count());
        }
    }
}