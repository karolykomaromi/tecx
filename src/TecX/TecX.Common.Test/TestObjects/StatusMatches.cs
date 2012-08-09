using System.Collections.Generic;
using TecX.Common.Specifications;

namespace TecX.Common.Test.TestObjects
{
    internal class StatusMatches : CompareToValueSpecification<SearchTestEntity, Status>
    {
        public override string Description
        {
            get { return "StatusMatches"; }
        }

        public StatusMatches()
        {
        }

        public StatusMatches(Status status)
        {
            this.Value = status;
        }

        protected override bool IsMatchCore(SearchTestEntity candidate, ICollection<ISpecification<SearchTestEntity>> matchedSpecifications)
        {
            return this.Value == candidate.Status;
        }
    }
}