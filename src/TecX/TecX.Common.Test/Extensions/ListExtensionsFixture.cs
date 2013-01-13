namespace TecX.Common.Test.Extensions
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Common.Extensions.Collections;

    [TestClass]
    public class ListExtensionsFixture
    {
        [TestMethod]
        public void CanAddRange()
        {
            IList<string> strings = new List<string> { "1", "2", "3" };

            var moreStrings = new[] { "4", "5" };

            strings.AddRange(moreStrings);

            Assert.AreEqual(5, strings.Count);
            Assert.IsTrue(strings.Contains("1"));
            Assert.IsTrue(strings.Contains("2"));
            Assert.IsTrue(strings.Contains("3"));
            Assert.IsTrue(strings.Contains("4"));
            Assert.IsTrue(strings.Contains("5"));
        }

        [TestMethod]
        public void CanConvertToEnumerable()
        {
            IList strings = new ArrayList { "1", "2", "3" };

            var enumerable = strings.AsEnumerable<string>();

            Assert.AreEqual(3, enumerable.Count());
        }
    }
}
