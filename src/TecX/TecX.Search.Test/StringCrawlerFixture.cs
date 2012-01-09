namespace TecX.Search.Test
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Search;
    using TecX.Search.Split;

    [TestClass]
    public class StringCrawlerFixture
    {
        [TestMethod]
        public void CanCrawlStringForOpeningAndClosingBraces()
        {
            StringCrawler crawler = new StringCrawler("(", ")");

            string pattern = "(123) (abc def)";

            crawler.Crawl(pattern.Split(new[] { Constants.Blank }, StringSplitOptions.RemoveEmptyEntries));

            Assert.IsTrue(crawler.Sequences.Contains(new StringSplitParameter("123")));
            Assert.IsTrue(crawler.Sequences.Contains(new StringSplitParameter("abc def")));
        }
    }
}