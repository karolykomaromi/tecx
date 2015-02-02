namespace TecX.Search.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Search.Data.EF;

    [TestClass]
    public class SqlServerHelperFixture
    {
        [TestMethod]
        public void CanReplaceWindowsWildcardsWithSqlWildcards()
        {
            string pattern = "?abc*x";

            string sql = SqlServerHelper.AdjustWildcards(pattern);

            Assert.AreEqual("_abc%x%", sql);
        }
    }
}
