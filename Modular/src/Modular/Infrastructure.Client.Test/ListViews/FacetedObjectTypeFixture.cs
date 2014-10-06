namespace Infrastructure.Client.Test.ListViews
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Infrastructure.Client.Test.TestObjects;
    using Infrastructure.ListViews;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FacetedObjectTypeTest
    {
        [TestMethod]
        public void Should_Ignore_Properties_Decorated_With_NotListViewRelevantAttribute()
        {
            Type type = new FacetedObjectType<HasNotListViewRelevantAttribute>(new List<Facet>());

            PropertyInfo property = type.GetProperty("ShouldBeIgnored");

            Assert.IsNull(property);
        }

        [TestMethod]
        public void Should_Find_Standard_Public_Property()
        {
            Type type = new FacetedObjectType<HasClassLevelProperty>(new List<Facet>());

            Assert.IsNotNull(type.GetProperty("ShouldBeFound"));
        }

        [TestMethod]
        public void Should_Find_Dynamic_Property()
        {
            Type type = new FacetedObjectType<HasNoProperties>(
                new List<Facet>
                    {
                        new Facet { PropertyName = "DynamicProperty", PropertyType = typeof(string) }
                    });

            Assert.IsNotNull(type.GetProperty("DynamicProperty"));
        }

        [TestMethod]
        public void Should_Find_Mixed_Standard_And_Dynamic_Properties()
        {
            Type type = new FacetedObjectType<HasClassLevelProperty>(new List<Facet>
                {
                    new Facet { PropertyName = "DynamicProperty", PropertyType = typeof(string) }
                });

            PropertyInfo[] properties = type.GetProperties();

            Assert.AreEqual(2, properties.Length);

            Assert.IsTrue(properties.Any(p => string.Equals("DynamicProperty", p.Name)));
            Assert.IsTrue(properties.Any(p => string.Equals("ShouldBeFound", p.Name)));
        }

        [TestMethod]
        public void Should_Find_Dynamic_Property_With_Single_Blank_As_Name()
        {
            Type type = new FacetedObjectType<HasNoProperties>(
                new List<Facet>
                    {
                        new Facet { PropertyName = " ", PropertyType = typeof(string) }
                    });

            Assert.IsNull(type.GetProperty(" "));
        }
    }
}
