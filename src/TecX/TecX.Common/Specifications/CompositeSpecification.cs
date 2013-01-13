namespace TecX.Common.Specifications
{
    using System.Collections.Generic;

    public abstract class CompositeSpecification<T> : Specification<T>
    {
        private readonly List<Specification<T>> children;

        protected CompositeSpecification()
        {
            this.children = new List<Specification<T>>();
        }

        public IEnumerable<Specification<T>> Children
        {
            get
            {
                return this.children;
            }
        }

        public int Count
        {
            get
            {
                return this.children.Count;
            }
        }

        public override sealed void Accept(SpecificationVisitor<T> visitor)
        {
            Guard.AssertNotNull(visitor, "visitor");

            visitor.Visit(this);
        }

        protected void Add(Specification<T> specification)
        {
            Guard.AssertNotNull(specification, "specification");

            this.children.Add(specification);
        }

        protected void AddRange(IEnumerable<Specification<T>> specifications)
        {
            Guard.AssertNotNull(specifications, "specifications");

            this.children.AddRange(specifications);
        }
    }
}