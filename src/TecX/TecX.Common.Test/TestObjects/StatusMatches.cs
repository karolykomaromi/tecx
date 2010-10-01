﻿using TecX.Common.Specifications;

namespace TecX.Common.Test.TestObjects
{
    internal class StatusMatches : CompareToValueSpecification<SearchTestEntity, Status>
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
}