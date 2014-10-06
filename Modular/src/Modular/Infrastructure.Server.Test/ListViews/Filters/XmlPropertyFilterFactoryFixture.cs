namespace Infrastructure.Server.Test.ListViews.Filters
{
    using System.Xml.Linq;
    using Infrastructure.ListViews.Filter;
    using Xunit;

    public class XmlPropertyFilterFactoryFixture
    {
        [Fact]
        public void Should_Create_CompositeFilter_From_Xml()
        {
            XElement document = new XElement(
                                    XmlPropertyFilterFactory.Names.Elements.Root,
                                    new XElement(
                                        XmlPropertyFilterFactory.Names.Elements.Property, 
                                        new XAttribute(XmlPropertyFilterFactory.Names.Attributes.Name, "Foo")),
                                    new XElement(
                                        XmlPropertyFilterFactory.Names.Elements.Property, 
                                        new XAttribute(XmlPropertyFilterFactory.Names.Attributes.Name, "Bar")));

            IPropertyFilter filter = new XmlPropertyFilterFactory().Create(document);

            Assert.True(filter.IsMatch("Foo"));
            Assert.True(filter.IsMatch("Bar"));
        }
    }
}
