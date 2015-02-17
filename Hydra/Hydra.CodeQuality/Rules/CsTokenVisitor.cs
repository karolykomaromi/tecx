namespace Hydra.CodeQuality.Rules
{
    using StyleCop;
    using StyleCop.CSharp;

    public abstract class CsTokenVisitor : Visitor, ICsTokenVisitor
    {
        public static readonly ICsTokenVisitor Null = new NullCsTokenVisitor();

        protected CsTokenVisitor(SourceAnalyzer sourceAnalyzer)
            : base(sourceAnalyzer)
        {
        }

        public abstract void Visit(Node<CsToken> tokenNode, Node<CsToken> parentTokenNode);
    }
}