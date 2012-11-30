namespace TecX.Common.Specifications
{
    public class And<T> : CompositeSpecification<T>
    {
        public And(Specification<T> specification, Specification<T> other)
        {
            Guard.AssertNotNull(specification, "specification");
            Guard.AssertNotNull(other, "other");

            this.Include(specification);
            this.Include(other);
        }

        public override string Description
        {
            get { return " && "; }
        }

        public override bool IsSatisfiedBy(T candidate)
        {
            Guard.AssertNotNull(candidate, "candidate");

            foreach (Specification<T> specification in this.Children)
            {
                if (!specification.IsSatisfiedBy(candidate))
                {
                    return false;
                }
            }

            return true;
        }

        private void Include(Specification<T> specification)
        {
            And<T> and = specification as And<T>;

            if (and != null)
            {
                this.AddRange(and.Children);
            }
            else
            {
                this.Add(specification);
            }
        }
    }
}