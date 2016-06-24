namespace Cars.CodeQuality.Rules
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using StyleCop;
    using StyleCop.CSharp;

    public class CompositeTokenVisitor : ITokenVisitor
    {
        private readonly List<ITokenVisitor> visitors;

        public CompositeTokenVisitor(params ITokenVisitor[] visitors)
        {
            Contract.Requires(visitors != null);

            this.visitors = new List<ITokenVisitor>(visitors ?? new ITokenVisitor[0]);
        }

        public void Visit(Node<CsToken> tokenNode, Node<CsToken> parentTokenNode)
        {
            foreach (ITokenVisitor visitor in this.visitors)
            {
                visitor.Visit(tokenNode, parentTokenNode);
            }
        }
    }
}