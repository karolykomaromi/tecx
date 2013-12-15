namespace TecX.Search.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Search.Split;

    [TestClass]
    public class StringSplitParameterCollectionFixture
    {
        [TestMethod]
        public void CannotAddDuplicateParameter()
        {
            StringSplitParameter p1 = new StringSplitParameter("1");
            StringSplitParameter p2 = new StringSplitParameter("1");

            StringSplitParameterCollection collection = new StringSplitParameterCollection();

            collection.Add(p1);

            Assert.AreEqual(1, collection.Count);

            collection.Add(p2);

            Assert.AreEqual(1, collection.Count);
        } 
    }
}
