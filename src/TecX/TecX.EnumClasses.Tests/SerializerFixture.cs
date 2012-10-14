namespace TecX.EnumClasses.Tests
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.EnumClasses.Tests.TestObjects;

    [TestClass]
    public class SerializerFixture
    {
        [TestMethod]
        public void Should_SerializeComplexObject_AsSimpleValue()
        {
            SerializeMe sm = new SerializeMe { SortOrder = SortOrder.Descending };

            IDataContractSurrogate surrogate = new EnumerationClassesSurrogate();

            DataContractSerializer serializer = new DataContractSerializer(typeof(SerializeMe), null, int.MaxValue, false, false, surrogate);

            MemoryStream stream = new MemoryStream();

            serializer.WriteObject(stream, sm);

            stream.Position = 0;

            TextReader reader = new StreamReader(stream);

            string xml = reader.ReadToEnd();

            stream.Position = 0;

            DataContractSerializer deserializer = new DataContractSerializer(typeof(SerializeMe2));

            SerializeMe2 deserialized = (SerializeMe2)deserializer.ReadObject(stream);

            Assert.AreEqual(SortOrderEnum.Descending, deserialized.SortOrder);
        }

        [TestMethod]
        public void Should_DeserializeSimpleValue_AsComplexObject()
        {
            SerializeMe2 sm = new SerializeMe2 { SortOrder = SortOrderEnum.Descending };

            DataContractSerializer serializer = new DataContractSerializer(typeof(SerializeMe2));

            MemoryStream stream = new MemoryStream();

            serializer.WriteObject(stream, sm);

            stream.Position = 0;

            TextReader reader = new StreamReader(stream);

            string xml = reader.ReadToEnd();

            stream.Position = 0;

            IDataContractSurrogate surrogate = new EnumerationClassesSurrogate();

            DataContractSerializer deserializer = new DataContractSerializer(typeof(SerializeMe), null, int.MaxValue, false, false, surrogate);

            SerializeMe deserialized = (SerializeMe)deserializer.ReadObject(stream);

            Assert.AreEqual(SortOrder.Descending, deserialized.SortOrder);
        }

        [TestMethod]
        public void Should_GenerateEnum_From_EnumerationClass()
        {
            var generator = new EnumGenerator();

            Type enumType;
            Assert.IsTrue(generator.TryGetEnumByType(typeof(SortOrder), out enumType));
        }
    }
}
