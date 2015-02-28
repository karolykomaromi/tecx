namespace Hydra.CodeQuality.Rules
{
    using System.Diagnostics.CodeAnalysis;
    using StyleCop;
    using StyleCop.CSharp;

    public abstract class QueryClauseVisitor : Visitor, IQueryClauseVisitor
    {
        public static readonly IQueryClauseVisitor Null = new NullQueryClauseVisitor();

        protected QueryClauseVisitor(SourceAnalyzer sourceAnalyzer)
            : base(sourceAnalyzer)
        {
        }

        [SuppressMessage("Hydra.CodeQuality.CodeQualityRules", "HD1001:MethodMustNotHaveMoreThanFourParameters", Justification = "Needed to work with StyleCop CodeWalkerQueryClauseVisitor<T>")]
        public abstract bool Visit(
            QueryClause clause,
            QueryClause parentClause,
            Expression parentExpression,
            Statement parentStatement,
            CsElement parentElement,
            object context);
    }
}