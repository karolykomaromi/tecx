namespace Cars.CodeQuality.Rules
{
    using StyleCop;
    using StyleCop.CSharp;

    public interface ITokenVisitor
    {
        void Visit(Node<CsToken> tokenNode, Node<CsToken> parentTokenNode);
    }
}