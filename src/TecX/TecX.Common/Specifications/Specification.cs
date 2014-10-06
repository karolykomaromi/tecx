namespace TecX.Common.Specifications
{
    public abstract class Specification<T>
    {
        public abstract string Description { get; }

        public abstract bool IsSatisfiedBy(T candidate);

        public virtual void Accept(SpecificationVisitor<T> visitor)
        {
            Guard.AssertNotNull(visitor, "visitor");

            visitor.Visit(this);
        }

        public Specification<T> And(Specification<T> other)
        {
            Guard.AssertNotNull(other, "other");

            return new And<T>(this, other);
        }

        public Specification<T> Or(Specification<T> other)
        {
            Guard.AssertNotNull(other, "other");

            return new Or<T>(this, other);
        }
    }
}