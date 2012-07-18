namespace TecX.Search.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Search;

    [TestClass]
    public class SearchParameterCollectionFixture
    {
        [TestMethod]
        public void CanConvertToValueArray()
        {
            SearchParameterCollection searchParameters = new SearchParameterCollection();

            searchParameters.Add(new SearchParameter<string>("abc"));
            searchParameters.Add(new SearchParameter<int>(123));

            var values = searchParameters.GetParameterValues();

            Assert.AreEqual(2, values.Length);
            Assert.AreEqual("abc", values[0]);
            Assert.AreEqual(123, values[1]);
        }
    }
}
