using System.Text;

namespace TecX.Common.Specifications
{
    public class DescriptionVisitor<TCandidate> : ISpecificationVisitor<TCandidate>
    {
        private readonly StringBuilder _sb;

        public DescriptionVisitor()
        {
            _sb = new StringBuilder(1024);
        }

        public void Visit(ISpecification<TCandidate> specification)
        {
            Guard.AssertNotNull(specification, "specification");

            _sb.Append(specification.Description);
        }

        public void Visit(CompositeSpecification<TCandidate> specification)
        {
            Guard.AssertNotNull(specification, "specification");

            _sb.Append("(");
            specification.LeftSide.Accept(this);

            _sb.Append(" ");
            _sb.Append(specification.Description);
            _sb.Append(" ");

            specification.RightSide.Accept(this);
            _sb.Append(")");
        }

        /// <summary>
        /// Gets a textular representation of the specifications visited by this visitor
        /// </summary>
        /// <returns>The textular representation of all visited specifications</returns>
        public override string ToString()
        {
            return _sb.ToString();
        }
    }
}