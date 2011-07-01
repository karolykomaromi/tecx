using System;
using System.Collections.Generic;
using TecX.Common.Specifications;

namespace TecX.Common.Test.TestObjects
{
    internal class NumberBetween : RangeSpecification<SearchTestEntity>
    {
        public override string Description
        {
            get { return "NumberBetween"; }
        }

        public NumberBetween(IComparable lowerBound, IComparable upperBound)
            : base(lowerBound, upperBound)
        {
        }

        protected override bool IsMatchCore(SearchTestEntity candidate, ICollection<ISpecification<SearchTestEntity>> matchedSpecifications)
        {
            return LowerBound.CompareTo(candidate.Number) <= 0 &&
                   candidate.Number.CompareTo(UpperBound) <= 0;
        }
    }
}