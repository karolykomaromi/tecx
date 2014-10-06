namespace Infrastructure.ListViews.Filter
{
    using System.Diagnostics.Contracts;
    using System.Xml.Linq;

    public class XmlPropertyFilterFactory
    {
        public IPropertyFilter Create(XElement document)
        {
            Contract.Requires(document != null);

            CompositeFilter composite = new CompositeFilter();

            foreach (XElement element in document.Elements(Names.Elements.Property))
            {
                string propertyName;
                if (element.Attribute(Names.Attributes.Name) != null &&
                    !string.IsNullOrEmpty(propertyName = element.Attribute(Names.Attributes.Name).Value))
                {
                    IPropertyFilter filter = new PropertyFilter(propertyName);

                    composite.Add(filter);
                }
            }

            return composite;
        }

        public static class Names
        {
            public static class Elements
            {
                /// <summary>
                /// filters
                /// </summary>
                public static readonly string Root = "filters";

                /// <summary>
                /// property
                /// </summary>
                public static readonly string Property = "property";
            }

            public static class Attributes
            {
                /// <summary>
                /// name
                /// </summary>
                public static readonly string Name = "name";
            }
        }
    }
}