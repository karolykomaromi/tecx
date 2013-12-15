namespace TecX.Common.Test.TestObjects
{
    using System;

    using TecX.Common.Specifications;

    internal class NumberBetween : Specification<SearchTestEntity>
    {
        private readonly IComparable min;

        private readonly IComparable max;

        public override string Description
        {
            get { return "between"; }
        }

        public NumberBetween(IComparable min, IComparable max)
        {
            Guard.AssertNotNull(min, "min");
            Guard.AssertNotNull(max, "max");
            this.min = min;
            this.max = max;
        }

        public override bool IsSatisfiedBy(SearchTestEntity candidate)
        {
            return min.CompareTo(candidate.Number) <= 0 &&
                   candidate.Number.CompareTo(max) <= 0;
        }
    }
}