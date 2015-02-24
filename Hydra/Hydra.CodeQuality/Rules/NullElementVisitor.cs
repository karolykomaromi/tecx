namespace Hydra.CodeQuality.Rules
{
    using StyleCop.CSharp;

    public class NullElementVisitor : IElementVisitor
    {
        public bool Visit(CsElement element, CsElement parentelement, object context)
        {
            return true;
        }
    }
}