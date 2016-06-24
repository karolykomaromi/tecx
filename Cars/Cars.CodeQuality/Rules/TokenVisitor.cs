namespace Cars.CodeQuality.Rules
{
    using StyleCop;
    using StyleCop.CSharp;

    public abstract class TokenVisitor : Visitor, ITokenVisitor
    {
        public static readonly ITokenVisitor Null = new NullTokenVisitor();

        protected TokenVisitor(SourceAnalyzer sourceAnalyzer)
            : base(sourceAnalyzer)
        {
        }

        public abstract void Visit(Node<CsToken> tokenNode, Node<CsToken> parentTokenNode);
    }
}