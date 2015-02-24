namespace Hydra.CodeQuality.Rules
{
    using StyleCop.CSharp;

    public interface IStatementVisitor
    {
        bool Visit(
            Statement statement,
            Expression parentExpression,
            Statement parentStatement,
            CsElement parentElement,
            object context);
    }
}