using System.Collections.Generic;
using TecX.Common.Specifications;

namespace TecX.Common.Test.TestObjects
{
    internal class NumberMatches : CompareToValueSpecification<SearchTestEntity, int>
    {
        public override string Description
        {
            get { return "NumberMatches"; }
        }

        public NumberMatches(int number)
        {
            this.Value = number;
        }

        protected override bool IsMatchCore(SearchTestEntity candidate, ICollection<ISpecification<SearchTestEntity>> matchedSpecifications)
        {
            return candidate.Number == this.Value;
        }
    }
}