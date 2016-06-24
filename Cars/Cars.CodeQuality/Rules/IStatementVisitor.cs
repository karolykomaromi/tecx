namespace Cars.CodeQuality.Rules
{
    using System.Diagnostics.CodeAnalysis;
    using StyleCop.CSharp;

    public interface IStatementVisitor
    {
        [SuppressMessage("Cars.CodeQuality.CodeQualityRules", "DV1001:MethodMustNotHaveMoreThanFourParameters", Justification = "Needed to work with StyleCop CodeWalkerStatementVisitor<T>")]
        bool Visit(
            Statement statement,
            Expression parentExpression,
            Statement parentStatement,
            CsElement parentElement,
            object context);
    }
}