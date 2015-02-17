namespace Hydra.CodeQuality.Rules
{
    using StyleCop;
    using StyleCop.CSharp;

    public interface ICsTokenVisitor
    {
        void Visit(Node<CsToken> tokenNode, Node<CsToken> parentTokenNode);
    }
}