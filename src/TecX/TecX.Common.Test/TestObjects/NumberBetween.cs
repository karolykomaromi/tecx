using System;
using TecX.Common.Specifications;

namespace TecX.Common.Test.TestObjects
{
    internal class NumberBetween : RangeSpecification<SearchTestEntity>
    {
        public NumberBetween(IComparable lowerBound, IComparable upperBound)
            : base(lowerBound, upperBound)
        {
        }

        protected override bool IsMatchCore(SearchTestEntity candidate)
        {
            return LowerBound.CompareTo(candidate.Number) <= 0 &&
                   candidate.Number.CompareTo(UpperBound) <= 0;
        }
    }
}