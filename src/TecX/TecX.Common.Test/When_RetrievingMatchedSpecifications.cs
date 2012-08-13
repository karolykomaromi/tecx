namespace TecX.Common.Test
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Common.Specifications;
    using TecX.Common.Test.TestObjects;

    [TestClass]
    public class When_RetrievingMatchedSpecifications : Given_SpecificationComposite
    {
        private List<ISpecification<SearchTestEntity>> matchedSpecs;

        protected override void Act()
        {
            this.matchedSpecs = new List<ISpecification<SearchTestEntity>>();

            this.composite.IsMatch(new SearchTestEntity(), this.matchedSpecs);
        }

        [TestMethod]
        public void Then_MatchedSpecsAreReturnedInCollection()
        {
            Assert.AreEqual(this.s1, this.matchedSpecs.Single());
        }
    }
}