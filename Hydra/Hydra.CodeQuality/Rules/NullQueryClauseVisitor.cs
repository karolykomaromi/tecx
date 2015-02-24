namespace Hydra.CodeQuality.Rules
{
    using StyleCop.CSharp;

    public class NullQueryClauseVisitor : IQueryClauseVisitor
    {
        public bool Visit(
            QueryClause clause,
            QueryClause parentClause,
            Expression parentExpression,
            Statement parentStatement,
            CsElement parentElement,
            object context)
        {
            return true;
        }
    }
}