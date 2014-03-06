namespace Infrastructure.Client.Test.ListViews
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Infrastructure.ListViews;
    using Infrastructure.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FacetedObjectTypeFixture
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
                        new Facet { PropertyName = "DoesNotExist", PropertyType = typeof(string) }
                    });

            Assert.IsNotNull(type.GetProperty("DoesNotExist"));
        }
    }

    public class HasNoProperties
    {
    }

    public class HasClassLevelProperty
    {
        public string ShouldBeFound { get; set; }
    }

    public class HasNotListViewRelevantAttribute
    {
        [PropertyMeta(IsListViewRelevant = false)]
        public string ShouldBeIgnored { get; set; }
    }
}
