namespace TecX.EnumClasses.Test
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.EnumClasses.Test.TestObjects;

    [TestClass]
    public class DataContractJsonSerializerFixture
    {
        [TestMethod]
        public void Should_SerializeComplexObject_AsSimpleJsonValue()
        {
            SerializeMe sm = new SerializeMe { SortOrder = SortOrder.Descending };

            IDataContractSurrogate surrogate = new EnumerationClassesSurrogate();

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(SerializeMe), null, int.MaxValue, false, surrogate, false);

            MemoryStream stream = new MemoryStream();

            serializer.WriteObject(stream, sm);

            stream.Position = 0;

            TextReader reader = new StreamReader(stream);

            string json = reader.ReadToEnd();

            stream.Position = 0;

            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(SerializeMe2));

            SerializeMe2 deserialized = (SerializeMe2)deserializer.ReadObject(stream);

            Assert.AreEqual(SortOrderEnum.Descending, deserialized.SortOrder);
        }

        [TestClass]
        public class EnumGeneratorFixture
        {
            [TestMethod]
            public void Should_GenerateEnum_From_EnumerationClass()
            {
                var generator = new EnumGenerator();

                Type enumType;
                Assert.IsTrue(generator.TryGetEnumByType(typeof(SortOrder), out enumType));

                Assert.IsNotNull(enumType.GetField(SortOrder.Ascending.Name));
                Assert.IsNotNull(enumType.GetField(SortOrder.Descending.Name));
            }
        }

        [TestMethod]
        public void Should_DeserializeSimpleJsonValue_AsComplexObject()
        {
            SerializeMe2 sm = new SerializeMe2 { SortOrder = SortOrderEnum.Descending };

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(SerializeMe2));

            MemoryStream stream = new MemoryStream();

            serializer.WriteObject(stream, sm);

            stream.Position = 0;

            TextReader reader = new StreamReader(stream);

            string json = reader.ReadToEnd();

            stream.Position = 0;

            IDataContractSurrogate surrogate = new EnumerationClassesSurrogate();

            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(SerializeMe), null, int.MaxValue, false, surrogate, false);

            SerializeMe deserialized = (SerializeMe)deserializer.ReadObject(stream);

            Assert.AreEqual(SortOrder.Descending, deserialized.SortOrder);
        }
    }
}
