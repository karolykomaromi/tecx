namespace TecX.Common.Specifications
{
    using System.Text;

    public class DescriptionVisitor<TCandidate> : ISpecificationVisitor<TCandidate>
    {
        private readonly StringBuilder sb;

        public DescriptionVisitor()
        {
            this.sb = new StringBuilder(1024);
        }

        public void Visit(ISpecification<TCandidate> specification)
        {
            Guard.AssertNotNull(specification, "specification");

            this.sb.Append(specification.Description);
        }

        public void Visit(CompositeSpecification<TCandidate> specification)
        {
            Guard.AssertNotNull(specification, "specification");

            this.sb.Append("(");
            specification.LeftSide.Accept(this);

            this.sb.Append(" ");
            this.sb.Append(specification.Description);
            this.sb.Append(" ");

            specification.RightSide.Accept(this);
            this.sb.Append(")");
        }

        /// <summary>
        /// Gets a textular representation of the specifications visited by this visitor
        /// </summary>
        /// <returns>The textular representation of all visited specifications</returns>
        public override string ToString()
        {
            return this.sb.ToString();
        }
    }
}