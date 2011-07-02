using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Common.Specifications;
using TecX.Common.Test.TestObjects;
using TecX.TestTools;

namespace TecX.Common.Test
{
    [TestClass]
    public class SpecificationFixture
    {
        [TestMethod]
        public void CanSearch()
        {
            var entity = new SearchTestEntity { Number = 3, Text = "abc" };

            List<SearchTestEntity> repository = new List<SearchTestEntity> { entity };

            var result = repository.FindAll(new TextMatches("abc").And(new NumberBetween(0, 5)));

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());

            result = repository.FindAll(new TextMatches("xyz").And(new NumberBetween(0, 5)));

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void CanSearchWithCompareToValueSpecification()
        {
            var entity = new SearchTestEntity { Number = 3, Text = "abc" };

            List<SearchTestEntity> repository = new List<SearchTestEntity> { entity };

            var result = repository.FindAll(new NumberBetween(0, 5).And(new TextMatches("xyz")));

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }


        [TestMethod]
        public void CanCompareToValueWithIntSpecification()
        {
            var entity = new SearchTestEntity { Number = 3, Text = "abc" };

            List<SearchTestEntity> repository = new List<SearchTestEntity> { entity };

            var result = repository.FindAll(new NumberBetween(0, 5).And(new NumberMatches(3)).And(new TextMatches("abc")));

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void CanCompareToValueWithBoolSpecification()
        {
            var entity = new SearchTestEntity { Number = 3, Text = "abc", HasSomeFlag = true };

            List<SearchTestEntity> repository = new List<SearchTestEntity> { entity };

            var result = repository.FindAll(new NumberBetween(0, 5).And(new HasSomeFlag()));

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }
    }

    public abstract class Given_SpecificationComposite : GivenWhenThen
    {
        protected ISpecification<SearchTestEntity> _composite;

        protected ISpecification<SearchTestEntity> _s1;

        protected override void Given()
        {
            _s1 = new DummySpec1();

            var s2 = new DummySpec2();

            var s3 = new DummySpec3();

            _composite = _s1.Or(s2.And(s3));
        }
    }

    [TestClass]
    public class When_UsingVisitorToExtractDescription : Given_SpecificationComposite
    {
        private DescriptionVisitor<SearchTestEntity> _visitor;

        private string _descriptions;

        protected override void When()
        {
            _visitor = new DescriptionVisitor<SearchTestEntity>();

            _composite.Accept(_visitor);

            _descriptions = _visitor.ToString();
        }

        [TestMethod]
        public void Then_VisitorCrawlsGraphAndCollectsDescriptions()
        {
            Assert.AreEqual("(DummySpec1 OR (DummySpec2 AND DummySpec3))", _descriptions);
        }
    }

    [TestClass]
    public class When_RetrievingMatchedSpecifications : Given_SpecificationComposite
    {
        private List<ISpecification<SearchTestEntity>> _matchedSpecs;

        protected override void When()
        {
            _matchedSpecs = new List<ISpecification<SearchTestEntity>>();

            _composite.IsMatch(new SearchTestEntity(), _matchedSpecs);
        }

        [TestMethod]
        public void Then_MatchedSpecsAreReturnedInCollection()
        {
            Assert.AreEqual(_s1, _matchedSpecs.Single());
        }
    }
}
