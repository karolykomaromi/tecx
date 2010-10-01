using System;
using TecX.Common.Specifications;

namespace TecX.Common.Test.TestObjects
{
    internal class NumberBetween : RangeSpecification<SearchTestEntity>
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
}