namespace Infrastructure.Client.Test.ListViews
{
    using System.Reflection;
    using Infrastructure.ListViews;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class DynamicPropertyFixture
    {
        [TestClass]
        public class DynamicPropertyInfoTest
        {
            [TestMethod]
            public void Should_Get_Property_Value()
            {
                FacetedViewModel vm = new FacetedViewModel();

                string propertyName = "DynamicProperty";

                Facet facet = new Facet { PropertyName = propertyName, PropertyType = typeof(string) };

                vm.AddFacet(facet);

                vm[propertyName] = "Foo";

                PropertyInfo property = vm.GetCustomType().GetProperty(propertyName);

                Assert.AreEqual("Foo", property.GetValue(vm, null));
            }

            [TestMethod]
            public void Should_Set_Property_Value()
            {
                FacetedViewModel vm = new FacetedViewModel();

                string propertyName = "DynamicProperty";

                Facet facet = new Facet { PropertyName = propertyName, PropertyType = typeof(string) };

                vm.AddFacet(facet);

                PropertyInfo property = vm.GetCustomType().GetProperty(propertyName);

                property.SetValue(vm, "Foo", null);

                Assert.AreEqual("Foo", (string)vm[propertyName]);
            }
        }
    }
}
