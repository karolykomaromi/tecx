using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TecX.Common.Specifications;

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

    public class SearchTestEntity
    {
        public int Number { get; set; }

        public string Text { get; set; }

        public Status Status { get; set; }

        public bool HasSomeFlag { get; set; }
    }

    public class TextMatches : CompareToValueSpecification<SearchTestEntity, string>
    {
        public TextMatches()
        {
        }

        public TextMatches(string text)
        {
            this.Value = text;
        }

        public override bool IsSatisfiedBy(SearchTestEntity candidate)
        {
            return candidate.Text == this.Value;
        }
    }

    public class StatusMatches : CompareToValueSpecification<SearchTestEntity, Status>
    {
        public StatusMatches()
        {

        }

        public StatusMatches(Status status)
        {
            this.Value = status;
        }

        public override bool IsSatisfiedBy(SearchTestEntity candidate)
        {
            return this.Value == candidate.Status;
        }
    }

    public class HasSomeFlag : CompareToValueSpecification<SearchTestEntity, bool>
    {
        public override bool IsSatisfiedBy(SearchTestEntity candidate)
        {
            return candidate.HasSomeFlag;
        }
    }

    public class NumberBetween : RangeSpecification<SearchTestEntity>
    {
        public NumberBetween()
        {
        }

        public NumberBetween(IComparable lowerBound, IComparable upperBound)
        {
            this.LowerBound = lowerBound;
            this.UpperBound = upperBound;
        }

        public override bool IsSatisfiedBy(SearchTestEntity candidate)
        {
            return LowerBound.CompareTo(candidate.Number) <= 0 &&
                   candidate.Number.CompareTo(UpperBound) <= 0;
        }
    }

    public enum Status
    {
        Started,

        Done,
    }

    public class NumberMatches : CompareToValueSpecification<SearchTestEntity, int>
    {
        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberMatches"/> class.
        /// </summary>
        public NumberMatches()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberMatches"/> class.
        /// </summary>
        /// <param name="number">The number.</param>
        public NumberMatches(int number)
        {
            this.Value = number;
        }

        #endregion c'tor

        #region Specification Members

        /// <summary>
        /// Determines whether a candidate object satifies the specification
        /// </summary>
        /// <param name="candidate">The candidate.</param>
        /// <returns>
        /// 	<c>true</c> if the specification is satisfied by the
        /// candidate object; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsSatisfiedBy(SearchTestEntity candidate)
        {
            return candidate.Number == this.Value;
        }

        #endregion Specification Members
    }
}
