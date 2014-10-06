namespace TecX.Common.Test.Specifications
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Common.Specifications;
    using TecX.Common.Test.TestObjects;

    [TestClass]
    public class SpecificationFixture
    {
        [TestMethod]
        public void Should_Extract_Friendly_Description()
        {
            var composite = new DummySpec1().Or(new DummySpec2().And(new DummySpec3()));

            var visitor = new PrettyPrinter<SearchTestEntity>();

            composite.Accept(visitor);
            
            Assert.AreEqual("(dummy1 || (dummy2 && dummy3))", visitor.ToString());
        }

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
}
