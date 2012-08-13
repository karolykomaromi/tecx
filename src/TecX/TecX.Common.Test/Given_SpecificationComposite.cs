namespace TecX.Common.Test
{
    using TecX.Common.Specifications;
    using TecX.Common.Test.TestObjects;
    using TecX.TestTools;

    public abstract class Given_SpecificationComposite : ArrangeActAssert
    {
        protected ISpecification<SearchTestEntity> composite;

        protected ISpecification<SearchTestEntity> s1;

        protected override void Arrange()
        {
            this.s1 = new DummySpec1();

            var s2 = new DummySpec2();

            var s3 = new DummySpec3();

            this.composite = this.s1.Or(s2.And(s3));
        }
    }
}