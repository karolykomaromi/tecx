namespace TecX.Search.Test
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Search;

    [TestClass]
    public class SearchParameterHelperFixture
    {
        [TestMethod]
        public void CanReoderForNameAndTimeFrameSearch()
        {
            var searchParameters = new SearchParameterCollection
                {
                    new SearchParameter<DateTime>(DateTime.MaxValue),
                    new SearchParameter<string>("abc"),
                    new SearchParameter<DateTime>(DateTime.MinValue)
                };

            SearchParameterHelper.ReorderForInterfaceAndTimeFrameSearch(searchParameters);

            Assert.AreEqual("abc", searchParameters[0].Value);
            Assert.AreEqual(DateTime.MinValue, searchParameters[1].Value);
            Assert.AreEqual(DateTime.MaxValue, searchParameters[2].Value);
        }

        [TestMethod]
        public void CanIdentifyIfIsNotNameAndInTimeFrameSearch()
        {
            var searchParameters = new SearchParameterCollection();

            Assert.IsFalse(SearchParameterHelper.IsInterfaceAndTimeFrameSearch(searchParameters));
        }

        [TestMethod]
        public void CanIdentifyIfIsNotNameAfterDateSearch()
        {
            var searchParameters = new SearchParameterCollection();

            Assert.IsFalse(SearchParameterHelper.IsInterfaceAndDateSearch(searchParameters));
        }

        [TestMethod]
        public void CanIdentifyIfIsNameAndInTimeFrameSearch()
        {
            var searchParameters = new SearchParameterCollection
                {
                    new SearchParameter<DateTime>(DateTime.MaxValue),
                    new SearchParameter<string>("abc"),
                    new SearchParameter<DateTime>(DateTime.MinValue)
                };

            Assert.IsTrue(SearchParameterHelper.IsInterfaceAndTimeFrameSearch(searchParameters));
        }

        [TestMethod]
        public void CanIdentifyIfIsNameAfterDateSearch()
        {
            var searchParameters = new SearchParameterCollection { new SearchParameter<DateTime>(DateTime.MaxValue), new SearchParameter<string>("abc") };

            Assert.IsTrue(SearchParameterHelper.IsInterfaceAndDateSearch(searchParameters));
        }

        [TestMethod]
        public void CanIdentifyIfIsInTimeFrameSearch()
        {
            var searchParameters = new SearchParameterCollection { new SearchParameter<DateTime>(DateTime.MaxValue), new SearchParameter<DateTime>(DateTime.MinValue) };

            Assert.IsTrue(SearchParameterHelper.IsTimeFrameSearch(searchParameters));
        }

        [TestMethod]
        public void CanReoderForNameAfterDateSearch()
        {
            var searchParameters = new SearchParameterCollection { new SearchParameter<DateTime>(DateTime.MaxValue), new SearchParameter<string>("abc") };

            SearchParameterHelper.ReorderForInterfaceAndDateSearch(searchParameters);

            Assert.AreEqual("abc", searchParameters[0].Value);
            Assert.AreEqual(DateTime.MaxValue, searchParameters[1].Value);
        }

        [TestMethod]
        public void CanReoderForTimeFrameSearch()
        {
            var searchParameters = new SearchParameterCollection { new SearchParameter<DateTime>(DateTime.MaxValue), new SearchParameter<DateTime>(DateTime.MinValue) };

            SearchParameterHelper.ReorderForTimeFrameSearch(searchParameters);

            Assert.AreEqual(DateTime.MinValue, searchParameters[0].Value);
            Assert.AreEqual(DateTime.MaxValue, searchParameters[1].Value);
        }

        [TestMethod]
        public void CanIdentifyIfIsInterfaceSearch()
        {
            var searchParameters = new SearchParameterCollection { new SearchParameter<string>("1") };

            Assert.IsTrue(SearchParameterHelper.IsInterfaceSearch(searchParameters));
        }

        [TestMethod]
        public void CanIdentifyIfIsNotInterfaceSearch()
        {
            var searchParameters = new SearchParameterCollection();

            Assert.IsFalse(SearchParameterHelper.IsInterfaceSearch(searchParameters));
        }
    }
}
