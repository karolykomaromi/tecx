namespace TecX.Import.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CsvFieldFixture
    {
        [TestMethod]
        public void Should_MismatchNullAndField()
        {
            var empty = CsvField.Empty;

            Assert.IsFalse(empty == null);
        }

        [TestMethod]
        public void Should_Match_EmptyAndFieldWithSameValues()
        {
            var empty = new CsvField(-1, string.Empty, string.Empty);

            Assert.IsTrue(empty.Equals(CsvField.Empty));
        }
    }
}
