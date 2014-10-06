namespace TecX.Search.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Search.Split;

    [TestClass]
    public class OrStringSplitStrategyFixture
    {
        [TestMethod]
        public void CanSplitOredSearchTerms()
        {
            string pattern = @"abc |123| def 234|hij";

            var context = new StringSplitContext { StringToSplit = pattern };

            var strategy = new OrStringSplitStrategy();

            strategy.Split(context);

            Assert.IsTrue(context.SplitResults.Contains(new StringSplitParameter("abc 123 def")));
            Assert.IsTrue(context.SplitResults.Contains(new StringSplitParameter("234 hij")));
            Assert.AreEqual(string.Empty, context.StringToSplit);
        }
    }
}
