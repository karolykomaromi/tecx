using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

using TecX.TestTools.Test.TestObjects;

namespace TecX.TestTools.Test
{
    using System.Collections.Generic;
    using System.Text;

    using Newtonsoft.Json;

    using TecX.TestTools.AutoFixture;

    [TestClass]
    public class AutoFixtureExtensionsFixture
    {
        [TestMethod]
        public void CanGetReadonlyObjectFilled()
        {
            var fixture = new Fixture();

            var parent = fixture.CreateAnonymous<ComplexParent>();
            new AutoPropertiesCommand().Execute(
                parent.Child,
                new SpecimenContext(fixture.Compose()));

            Assert.IsFalse(string.IsNullOrEmpty(parent.Blub));
            Assert.AreNotEqual(0, parent.Bla);
            Assert.AreNotEqual(0, parent.Child.Bar);
            Assert.IsFalse(string.IsNullOrEmpty(parent.Child.Foo));
            Assert.AreNotEqual(0, parent.Child.Bar2);
        }

        [TestMethod]
        public void CanCustomizeFixtureToGetReadonlyObjectFilled()
        {
            var fixture = new Fixture();

            fixture.Customize<ComplexParent>(c =>
                c.Do(x => new AutoPropertiesCommand()
                    .Execute(x.Child, new SpecimenContext(fixture.Compose()))));

            var parent = fixture.CreateAnonymous<ComplexParent>();

            Assert.IsFalse(string.IsNullOrEmpty(parent.Blub));
            Assert.AreNotEqual(0, parent.Bla);
            Assert.AreNotEqual(0, parent.Child.Bar);
            Assert.IsFalse(string.IsNullOrEmpty(parent.Child.Foo));
            Assert.AreNotEqual(0, parent.Child.Bar2);
        }

        [TestMethod]
        public void CanCreateMoreRealisticValuesUsingObjectHydrator()
        {
            var fixture = new Fixture().Customize(new ObjectHydratorCustomization());

            var customer = fixture.CreateAnonymous<Customer>();

            string y = JsonConvert.SerializeObject(customer, Formatting.Indented);
        }
    }
}