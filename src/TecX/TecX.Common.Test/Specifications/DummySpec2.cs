namespace TecX.Common.Test.Specifications
{
    using TecX.Common.Test.TestObjects;

    public class DummySpec2 : AlwaysFalse<SearchTestEntity>
    {
        public override string Description
        {
            get { return "dummy2"; }
        }
    }
}