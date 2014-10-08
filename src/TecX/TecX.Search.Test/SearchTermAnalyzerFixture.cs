namespace TecX.Search.Test
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Search;

    [TestClass]
    public class SearchTermAnalyzerFixture
    {
        [TestMethod]
        public void WordBreakerFindsProperSearchParameters()
        {
            SearchTextAnalyzer wb = new SearchTextAnalyzer();

            string pattern = "2011-11-8 abc 2011-11-9 15:00";

            SearchParameterCollection searchParameters = wb.Process(pattern);

            Assert.AreEqual(3, searchParameters.Count);
            Assert.IsTrue(searchParameters.Contains(new SearchParameter<DateTime>(new DateTime(2011, 11, 8))));
            Assert.IsTrue(searchParameters.Contains(new SearchParameter<DateTime>(new DateTime(2011, 11, 9, 15, 0, 0))));
            Assert.IsTrue(searchParameters.Contains(new SearchParameter<string>("abc")));
        }
    }
}
