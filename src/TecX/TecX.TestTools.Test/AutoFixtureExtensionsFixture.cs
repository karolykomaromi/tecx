using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace TecX.TestTools.Test
{
    [TestClass]
    public class AutoFixtureExtensionsFixture
    {
        [TestMethod]
        [Ignore]
        public void CanFillExistingObject()
        {
            //TODO weberse CreateAnonymous is an extension method since AutoFixture 2.0
            //so you it is rather tricky to find it using reflection...

            var fixture = new Fixture();

            ComplexParent parent = fixture.CreateAnonymous<ComplexParent>();

            fixture.Fill(parent.Child);

            Assert.IsFalse(string.IsNullOrEmpty(parent.Blub));
            Assert.AreNotEqual(0, parent.Bla);
            Assert.AreNotEqual(0, parent.Child.Bar);
            Assert.IsFalse(string.IsNullOrEmpty(parent.Child.Foo));
            Assert.AreNotEqual(0, parent.Child.Bar2);
        }

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
    }

    internal class ComplexParent
    {
        public string Blub { get; set; }
        public int Bla { get; set; }

        private readonly ComplexChild _child;

        public ComplexChild Child
        {
            get { return _child; }
        }

        public ComplexParent()
        {
            _child = new ComplexChild();
        }
    }

    internal class ComplexChild
    {
        public string Foo { get; set; }
        public int Bar { get; set; }
        public int Bar2 { get; set; }
    }
}