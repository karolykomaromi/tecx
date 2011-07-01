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
        protected ISpecification<SearchTestEntity> composite;

        protected override void Given()
        {
            var s1 = new DummySpec1();

            var s2 = new DummySpec2();

            var s3 = new DummySpec3();

            composite = s1.And(s2.Or(s3));
        }
    }

    [TestClass]
    public class When_UsingVisitorToExtractDescription : Given_SpecificationComposite
    {
        private DescriptionVisitor<SearchTestEntity> visitor;

        private string descriptions;

        protected override void When()
        {
            visitor = new DescriptionVisitor<SearchTestEntity>();

            composite.Accept(visitor);

            descriptions = visitor.ToString();
        }

        [TestMethod]
        public void Then_VisitorCrawlsGraphAndCollectsDescriptions()
        {
            Assert.AreEqual("(DummySpec1 AND (DummySpec2 OR DummySpec3))", descriptions);
        }
    }
}
