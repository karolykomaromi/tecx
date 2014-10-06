namespace TecX.TestTools.Test
{
    using AutoFixture;

    using Newtonsoft.Json;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Kernel;

    using TestObjects;

    using Xunit;

    public class AutoFixtureExtensionsFixture
    {
        [Fact]
        public void CanGetReadonlyObjectFilled()
        {
            Fixture fixture = new Fixture();

            var parent = fixture.Create<ComplexParent>();
            new AutoPropertiesCommand().Execute(
                parent.Child,
                new SpecimenContext(fixture));

            Assert.False(string.IsNullOrEmpty(parent.Blub));
            Assert.NotEqual(0, parent.Bla);
            Assert.NotEqual(0, parent.Child.Bar);
            Assert.False(string.IsNullOrEmpty(parent.Child.Foo));
            Assert.NotEqual(0, parent.Child.Bar2);
        }

        [Fact]
        public void CanCustomizeFixtureToGetReadonlyObjectFilled()
        {
            var fixture = new Fixture();

            fixture.Customize<ComplexParent>(c =>
                c.Do(x => new AutoPropertiesCommand()
                    .Execute(x.Child, new SpecimenContext(fixture))));

            var parent = fixture.Create<ComplexParent>();

            Assert.False(string.IsNullOrEmpty(parent.Blub));
            Assert.NotEqual(0, parent.Bla);
            Assert.NotEqual(0, parent.Child.Bar);
            Assert.False(string.IsNullOrEmpty(parent.Child.Foo));
            Assert.NotEqual(0, parent.Child.Bar2);
        }

        [Fact]
        public void CanCreateMoreRealisticValuesUsingObjectHydrator()
        {
            var fixture = new Fixture().Customize(new ObjectHydratorCustomization());

            var customer = fixture.Create<Customer>();

            string y = JsonConvert.SerializeObject(customer, Formatting.Indented);
        }
    }
}