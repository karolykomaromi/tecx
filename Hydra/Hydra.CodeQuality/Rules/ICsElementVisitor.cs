namespace Hydra.CodeQuality.Rules
{
    using StyleCop.CSharp;

    public interface ICsElementVisitor
    {
        bool Visit(CsElement element, CsElement parentelement, object context);
    }
}