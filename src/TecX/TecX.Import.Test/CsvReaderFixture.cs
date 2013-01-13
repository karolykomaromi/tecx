namespace TecX.Import.Test
{
    using System;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CsvReaderFixture
    {
        private const string OneLine = "1;2;";

        private static readonly string OneLineWithHeader = "foo;bar;" + Environment.NewLine + "1;2;";

        [TestMethod]
        public void Should_FindFieldByIndex()
        {
            CsvReader reader = new CsvReaderBuilder().FromString(OneLine);

            CsvLine line = reader.Single();

            Assert.AreEqual(2, line.FieldCount);

            CsvField field;
            Assert.IsTrue(line.TryGetField(0, out field));
            Assert.AreEqual("1", field.Value);
            Assert.IsTrue(line.TryGetField(1, out field));
            Assert.AreEqual("2", field.Value);
        }

        [TestMethod]
        public void Should_FindFieldByHeader()
        {
            CsvReader reader = new CsvReaderBuilder().FromString(OneLineWithHeader).WithHeaders();

            CsvLine line = reader.Single();

            Assert.AreEqual(2, line.FieldCount);

            CsvField field;
            Assert.IsTrue(line.TryGetField("foo", out field));
            Assert.AreEqual("1", field.Value);
            Assert.IsTrue(line.TryGetField("bar", out field));
            Assert.AreEqual("2", field.Value);
        }

        [TestMethod]
        public void Should_IgnoreTrailingSeparator_When_ComparingAgainstNumberOfFieldsInHeader()
        {
            CsvReader reader = new CsvReaderBuilder().FromString(OneLineWithHeader).WithHeaders();

            CsvLine line = reader.Single();

            Assert.AreEqual(2, line.FieldCount);
        }
    }
}