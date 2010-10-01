using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TecX.Common.Specifications;
using TecX.Common.Test.TestObjects;

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

            var result = repository.FindAll(new TextMatches("abc").And<NumberBetween>(0, 5));

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());

            result = repository.FindAll(new TextMatches("xyz").And<NumberBetween>(0, 5));

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void CanSearchWithCompareToValueSpecification()
        {
            var entity = new SearchTestEntity { Number = 3, Text = "abc" };

            List<SearchTestEntity> repository = new List<SearchTestEntity> { entity };

            var result = repository.FindAll(new NumberBetween(0, 5).And<TextMatches>("xyz"));

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }


        [TestMethod]
        public void CanCompareToValueWithIntSpecification()
        {
            var entity = new SearchTestEntity { Number = 3, Text = "abc" };

            List<SearchTestEntity> repository = new List<SearchTestEntity> { entity };

            var result = repository.FindAll(new NumberBetween(0, 5).And<NumberMatches>(3).And<TextMatches>("abc"));

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void CanCompareToValueWithBoolSpecification()
        {
            var entity = new SearchTestEntity { Number = 3, Text = "abc", HasSomeFlag = true };

            List<SearchTestEntity> repository = new List<SearchTestEntity> { entity };

            var result = repository.FindAll(new NumberBetween(0, 5).And<HasSomeFlag>(true));

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }
    }
}
