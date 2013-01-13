namespace TecX.Common.Specifications
{
    public class Or<T> : CompositeSpecification<T>
    {
        public Or(Specification<T> specification, Specification<T> other)
        {
            Guard.AssertNotNull(specification, "specification");
            Guard.AssertNotNull(other, "other");

            this.Include(specification);
            this.Include(other);
        }

        public override string Description
        {
            get { return " || "; }
        }

        public override bool IsSatisfiedBy(T candidate)
        {
            Guard.AssertNotNull(candidate, "candidate");

            foreach (Specification<T> specification in this.Children)
            {
                if (specification.IsSatisfiedBy(candidate))
                {
                    return true;
                }
            }

            return false;
        }

        private void Include(Specification<T> other)
        {
            Or<T> or = other as Or<T>;

            if (or != null)
            {
                this.AddRange(or.Children);
            }
            else
            {
                this.Add(other);
            }
        }
    }
}