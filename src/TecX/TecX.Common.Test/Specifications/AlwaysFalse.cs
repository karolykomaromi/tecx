namespace TecX.Common.Test.Specifications
{
    using TecX.Common.Specifications;

    public class AlwaysFalse<T> : Specification<T>
    {
        public override string Description
        {
            get { return "false"; }
        }

        public override bool IsSatisfiedBy(T candidate)
        {
            return false;
        }
    }
}