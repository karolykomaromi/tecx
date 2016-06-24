namespace Cars.CodeQuality.Rules
{
    using System.Diagnostics.CodeAnalysis;
    using StyleCop.CSharp;

    public interface IQueryClauseVisitor
    {
        [SuppressMessage("Cars.CodeQuality.CodeQualityRules", "DV1001:MethodMustNotHaveMoreThanFourParameters", Justification = "Needed to work with StyleCop CodeWalkerQueryClauseVisitor<T>")]
        bool Visit(
            QueryClause clause,
            QueryClause parentClause,
            Expression parentExpression,
            Statement parentStatement,
            CsElement parentElement,
            object context);
    }
}