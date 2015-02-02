namespace TecX.Common.Specifications
{
    public abstract class SpecificationVisitor<T>
    {
        public abstract void Visit(Specification<T> specification);

        public abstract void Visit(CompositeSpecification<T> specification);
    }
}