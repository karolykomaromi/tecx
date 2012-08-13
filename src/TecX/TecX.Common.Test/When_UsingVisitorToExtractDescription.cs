namespace TecX.Common.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Common.Specifications;
    using TecX.Common.Test.TestObjects;

    [TestClass]
    public class When_UsingVisitorToExtractDescription : Given_SpecificationComposite
    {
        private DescriptionVisitor<SearchTestEntity> visitor;

        private string descriptions;

        protected override void Act()
        {
            this.visitor = new DescriptionVisitor<SearchTestEntity>();

            this.composite.Accept(this.visitor);

            this.descriptions = this.visitor.ToString();
        }

        [TestMethod]
        public void Then_VisitorCrawlsGraphAndCollectsDescriptions()
        {
            Assert.AreEqual("(DummySpec1 OR (DummySpec2 AND DummySpec3))", this.descriptions);
        }
    }
}