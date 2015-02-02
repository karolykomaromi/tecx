namespace TecX.Common.Test.TestObjects
{
    using TecX.Common.Specifications;

    internal class StatusMatches : Specification<SearchTestEntity>
    {
        private readonly Status status;

        public override string Description
        {
            get { return "status is"; }
        }

        public StatusMatches(Status status)
        {
            this.status = status;
        }

        public override bool IsSatisfiedBy(SearchTestEntity candidate)
        {
            return this.status == candidate.Status;
        }
    }
}