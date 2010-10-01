using TecX.Common.Specifications;

namespace TecX.Common.Test.TestObjects
{
    internal class HasSomeFlag : CompareToValueSpecification<SearchTestEntity, bool>
    {
        public override bool IsSatisfiedBy(SearchTestEntity candidate)
        {
            return candidate.HasSomeFlag;
        }
    }
}