namespace Hydra.CodeQuality.Rules
{
    using StyleCop;
    using StyleCop.CSharp;

    public class NullCsTokenVisitor : ICsTokenVisitor
    {
        public void Visit(Node<CsToken> tokenNode, Node<CsToken> parentTokenNode)
        {
        }
    }
}