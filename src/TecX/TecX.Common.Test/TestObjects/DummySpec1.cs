namespace TecX.Common.Test.TestObjects
{
    using TecX.Common.Specifications;

    public class DummySpec1 : Specification<SearchTestEntity>
    {
        public override string Description
        {
            get { return "dummy1"; }
        }

        public override bool IsSatisfiedBy(SearchTestEntity candidate)
        {
            return true;
        }
    }
}