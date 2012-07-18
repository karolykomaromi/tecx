using TecX.Common.Specifications;

namespace TecX.Common.Test.TestObjects
{
    public class DummySpec2 : AlwaysFalseSpecification<SearchTestEntity>
    {
        public override string Description
        {
            get { return "DummySpec2"; }
        }
    }
}