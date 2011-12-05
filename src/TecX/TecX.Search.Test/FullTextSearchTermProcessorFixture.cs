namespace TecX.Search.Test
{
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Search;

    [TestClass]
    public class FullTextSearchTermProcessorFixture
    {
        [TestMethod]
        public void CanIdentifySearchTerms()
        {
            var fullText = new FullTextSearchTermProcessor();

            Message msg = new Message { Id = 1, MessageText = "mmm111mmm", Source = "mmm" };

            var searchTerms = fullText.Analyze(new[] { msg }).ToList();

            Assert.AreEqual(2, searchTerms.Count);
            Assert.AreEqual("mmm", searchTerms[0].Text);
            Assert.AreEqual("111", searchTerms[1].Text);
        }
    }
}
