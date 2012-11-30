namespace TecX.Common.Test.Specifications
{
    using TecX.Common.Test.TestObjects;

    public class DummySpec3 : AlwaysFalse<SearchTestEntity>
    {
        public override string Description
        {
            get { return "dummy3"; }
        }
    }
}