namespace TecX.EnumClasses.Tests
{
    using System.IO;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Newtonsoft.Json;

    using TecX.EnumClasses.Tests.TestObjects;

    [TestClass]
    public class JsonSerializerFixture
    {
        [TestMethod]
        public void Should_BeAbleToConvertSimpleValue_BackAndForth_WithJsonNET()
        {
            SerializeMe2 sm2 = new SerializeMe2 { SortOrder = SortOrderEnum.Descending };

            JsonSerializer serializer = new JsonSerializer();

            StringBuilder sb = new StringBuilder(100);

            TextWriter writer = new StringWriter(sb);

            serializer.Serialize(writer, sm2);

            string json = sb.ToString();

            TextReader reader = new StringReader(json);

            JsonSerializer deserializer = new JsonSerializer();

            deserializer.Converters.Add(new EnumerationClassesJsonConverter());

            SerializeMe sm = (SerializeMe)deserializer.Deserialize(reader, typeof(SerializeMe));

            Assert.AreEqual(SortOrder.Descending, sm.SortOrder);
        }

        [TestMethod]
        public void Should_BeAbleToConvertComplexValue_BackAndForth_WithJsonNET()
        {
            SerializeMe sm = new SerializeMe { SortOrder = SortOrder.Descending };

            JsonSerializer serializer = new JsonSerializer();

            serializer.Converters.Add(new EnumerationClassesJsonConverter());

            StringBuilder sb = new StringBuilder(100);

            TextWriter writer = new StringWriter(sb);

            serializer.Serialize(writer, sm);

            string json = sb.ToString();

            TextReader reader = new StringReader(json);

            JsonSerializer deserializer = new JsonSerializer();

            SerializeMe2 sm2 = (SerializeMe2)deserializer.Deserialize(reader, typeof(SerializeMe2));

            Assert.AreEqual(SortOrderEnum.Descending, sm2.SortOrder);
        }
    }
}
