namespace TecX.Common.Test.TestObjects
{
    using TecX.Common.Specifications;

    internal class NumberMatches : Specification<SearchTestEntity>
    {
        private readonly int i;

        public override string Description
        {
            get { return "equals"; }
        }

        public NumberMatches(int i)
        {
            this.i = i;
        }

        public override bool IsSatisfiedBy(SearchTestEntity candidate)
        {
            return candidate.Number == this.i;
        }
    }
}