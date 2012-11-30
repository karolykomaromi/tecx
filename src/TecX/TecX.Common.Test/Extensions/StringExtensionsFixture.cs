namespace TecX.Common.Test.Extensions
{
    using System.Globalization;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Common.Extensions.Primitives;

    [TestClass]
    public class StringExtensionsFixture
    {
        [TestMethod]
        public void CanConvertStringPortionsToUpperCase()
        {
            string original = "abcdefg";

            string modified = original.ToUpper(0, 7, CultureInfo.CurrentCulture);

            Assert.AreEqual("ABCDEFG", modified);

            modified = original.ToUpper(1, 5, CultureInfo.CurrentCulture);

            Assert.AreEqual("aBCDEFg", modified);

            modified = original.ToUpper(3, 1234, CultureInfo.CurrentCulture);

            Assert.AreEqual("abcDEFG", modified);

            modified = original.ToUpper(0, 2, CultureInfo.CurrentCulture);

            Assert.AreEqual("ABcdefg", modified);
        }
    }
}
