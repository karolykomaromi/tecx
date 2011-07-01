using TecX.Common.Specifications;

namespace TecX.Common.Test.TestObjects
{
    internal class HasSomeFlag : CompareToValueSpecification<SearchTestEntity, bool>
    {
        protected override bool IsMatchCore(SearchTestEntity candidate)
        {
            return candidate.HasSomeFlag;
        }
    }
}