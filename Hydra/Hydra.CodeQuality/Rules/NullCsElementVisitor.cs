namespace Hydra.CodeQuality.Rules
{
    using StyleCop.CSharp;

    public class NullCsElementVisitor : ICsElementVisitor
    {
        public bool Visit(CsElement element, CsElement parentelement, object context)
        {
            return true;
        }
    }
}