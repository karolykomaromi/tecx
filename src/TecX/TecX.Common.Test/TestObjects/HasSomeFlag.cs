namespace TecX.Common.Test.TestObjects
{
    using TecX.Common.Specifications;

    internal class HasSomeFlag : Specification<SearchTestEntity>
    {
        public override string Description
        {
            get { return "has flag"; }
        }

        public override bool IsSatisfiedBy(SearchTestEntity candidate)
        {
            return candidate.HasSomeFlag;
        }
    }
}