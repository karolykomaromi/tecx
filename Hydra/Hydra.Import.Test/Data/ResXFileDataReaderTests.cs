namespace Hydra.Import.Test.Data
{
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using Hydra.Import.Data;
    using Hydra.Infrastructure.I18n;
    using Xunit;

    public class ResXFileDataReaderTests
    {
        [Fact]
        public void Should_Read_ResourceItems_From_Stream()
        {
            using (Stream stream = new MemoryStream())
            {
                XDocument document = new XDocument(
                    new XElement(
                        "root",
                        new XElement("data", new XAttribute("name", "Foo"), new XElement("value", "1")),
                        new XElement("data", new XAttribute("name", "Bar"), new XElement("value", "2"))));

                document.Save(stream);
                stream.Position = 0;

                IDataReader<ResourceItem> sut = new ResXFileDataReader(stream, Cultures.GermanGermany);

                var actual = sut.ToArray();

                Assert.Equal(2, actual.Length);
                Assert.Equal("Foo", actual[0].Name);
                Assert.Equal("1", actual[0].Value);
                Assert.Equal("Bar", actual[1].Name);
                Assert.Equal("2", actual[1].Value);
                Assert.Equal(Cultures.GermanGermany, actual[1].Language);
            }
        }
    }
}
