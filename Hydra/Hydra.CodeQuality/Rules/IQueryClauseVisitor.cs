namespace Hydra.CodeQuality.Rules
{
    using StyleCop.CSharp;

    public interface IQueryClauseVisitor
    {
        bool Visit(
            QueryClause clause,
            QueryClause parentClause,
            Expression parentExpression,
            Statement parentStatement,
            CsElement parentElement,
            object context);
    }
}