using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ploeh.AutoFixture;

namespace TecX.TestTools.Test
{
    [TestClass]
    public class AutoFixtureExtensionsFixture
    {
        [TestMethod]
        public void CanFillExistingObject()
        {
            var fixture = new Fixture();

            ComplexParent parent = fixture.CreateAnonymous<ComplexParent>();

            fixture.Fill(parent.Child);

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