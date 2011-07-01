using TecX.Common.Specifications;

namespace TecX.Common.Test.TestObjects
{
    public class DummySpec1 : AlwaysFalseSpecification<SearchTestEntity>
    {
        public override string Description
        {
            get { return "DummySpec1"; }
        }
    }
}