namespace Hydra.CodeQuality.Rules
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using StyleCop;
    using StyleCop.CSharp;

    public class CompositeCsTokenVisitor : ICsTokenVisitor
    {
        private readonly List<ICsTokenVisitor> visitors;

        public CompositeCsTokenVisitor(params ICsTokenVisitor[] visitors)
        {
            Contract.Requires(visitors != null);

            this.visitors = new List<ICsTokenVisitor>(visitors ?? new ICsTokenVisitor[0]);
        }

        public void Visit(Node<CsToken> tokenNode, Node<CsToken> parentTokenNode)
        {
            foreach (ICsTokenVisitor visitor in this.visitors)
            {
                visitor.Visit(tokenNode, parentTokenNode);
            }
        }
    }
}