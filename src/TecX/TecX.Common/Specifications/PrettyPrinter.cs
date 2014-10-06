namespace TecX.Common.Specifications
{
    using System.Text;

    public class PrettyPrinter<T> : SpecificationVisitor<T>
    {
        private readonly StringBuilder sb;

        public PrettyPrinter()
        {
            this.sb = new StringBuilder(1024);
        }

        public override void Visit(Specification<T> specification)
        {
            Guard.AssertNotNull(specification, "specification");

            this.sb.Append(specification.Description);
        }

        public override void Visit(CompositeSpecification<T> specification)
        {
            Guard.AssertNotNull(specification, "specification");

            this.sb.Append("(");

            foreach (Specification<T> child in specification.Children)
            {
                child.Accept(this);
                sb.Append(specification.Description);
            }

            int l = specification.Description.Length;

            this.sb.Remove(this.sb.Length - l, l);

            this.sb.Append(")");
        }

        public override string ToString()
        {
            return this.sb.ToString();
        }
    }
}