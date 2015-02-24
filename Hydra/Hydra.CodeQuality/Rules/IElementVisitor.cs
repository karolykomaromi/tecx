namespace Hydra.CodeQuality.Rules
{
    using StyleCop.CSharp;

    public interface IElementVisitor
    {
        bool Visit(CsElement element, CsElement parentelement, object context);
    }
}