namespace Hydra.CodeQuality.Rules
{
    using StyleCop;
    using StyleCop.CSharp;

    public abstract class QueryClauseVisitor : Visitor, IQueryClauseVisitor
    {
        public static readonly IQueryClauseVisitor Null = new NullQueryClauseVisitor();

        protected QueryClauseVisitor(SourceAnalyzer sourceAnalyzer)
            : base(sourceAnalyzer)
        {
        }

        public abstract bool Visit(
            QueryClause clause,
            QueryClause parentClause,
            Expression parentExpression,
            Statement parentStatement,
            CsElement parentElement,
            object context);
    }
}