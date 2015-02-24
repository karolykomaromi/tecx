namespace Hydra.CodeQuality.Rules
{
    using StyleCop;
    using StyleCop.CSharp;

    public class NullTokenVisitor : ITokenVisitor
    {
        public void Visit(Node<CsToken> tokenNode, Node<CsToken> parentTokenNode)
        {
        }
    }
}