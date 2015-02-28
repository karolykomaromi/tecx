namespace Hydra.CodeQuality.Rules
{
    using System.Diagnostics.CodeAnalysis;
    using StyleCop.CSharp;

    public interface IStatementVisitor
    {
        [SuppressMessage("Hydra.CodeQuality.CodeQualityRules", "HD1001:MethodMustNotHaveMoreThanFourParameters", Justification = "Needed to work with StyleCop CodeWalkerStatementVisitor<T>")]
        bool Visit(
            Statement statement,
            Expression parentExpression,
            Statement parentStatement,
            CsElement parentElement,
            object context);
    }
}