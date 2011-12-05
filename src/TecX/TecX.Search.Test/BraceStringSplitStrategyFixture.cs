namespace TecX.Search.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Search.Split;

    [TestClass]
    public class BraceStringSplitStrategyFixture
    {
        [TestMethod]
        public void CanSplitStringByBraces()
        {
            var strategy = new BraceStringSplitStrategy();

            string pattern = "(abc ) (123 456)";

            var context = new StringSplitContext { StringToSplit = pattern };

            strategy.Split(context);

            Assert.IsTrue(context.SplitResults.Contains(new StringSplitParameter("abc")));
            Assert.IsTrue(context.SplitResults.Contains(new StringSplitParameter("123 456")));
            Assert.AreEqual(string.Empty, context.StringToSplit);
        }
    }
}
