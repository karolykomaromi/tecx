namespace TecX.EnumClasses.Test
{
    using System.IO;
    using System.Runtime.Serialization;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.EnumClasses.Test.TestObjects;

    [TestClass]
    public class FlagsFixture
    {
        [TestMethod]
        public void Should_SerializeFlags()
        {
            var wf = new WithFlags { Flags = EnumWithFlags.One | EnumWithFlags.Two };

            var serializer = new DataContractSerializer(typeof(WithFlags));

            MemoryStream stream = new MemoryStream();

            serializer.WriteObject(stream, wf);

            stream.Position = 0;

            TextReader reader = new StreamReader(stream);

            string xml = reader.ReadToEnd();
        }

        [TestMethod]
        public void Should1()
        {
            Assert.AreEqual(EnumWithFlags.One | EnumWithFlags.Four, (EnumWithFlags.One | EnumWithFlags.Four) & (EnumWithFlags.One | EnumWithFlags.Four));
            Assert.AreEqual(EnumWithFlags.One, (EnumWithFlags.One | EnumWithFlags.Four) & EnumWithFlags.One);
            Assert.AreEqual(EnumWithFlags.None, (EnumWithFlags.One | EnumWithFlags.Four) & EnumWithFlags.Two);
        }

        [TestMethod]
        public void Should2()
        {
            Assert.AreEqual(Numbers.One | Numbers.Four, (Numbers.One | Numbers.Four) & (Numbers.One | Numbers.Four));
            Assert.AreEqual(Numbers.One, (Numbers.One | Numbers.Four) & Numbers.One);
            Assert.AreEqual(Numbers.None, (Numbers.One | Numbers.Four) & Numbers.Two);
        }

        [TestMethod]
        public void Should3()
        {
            Assert.AreEqual(2, (0 | 1 | 2) & 2);
        }
    }
}
