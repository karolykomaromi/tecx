namespace Hydra.CodeQuality.Rules
{
    using System.Diagnostics.CodeAnalysis;
    using StyleCop.CSharp;

    public interface IExpressionVisitor
    {
        [SuppressMessage("Hydra.CodeQuality.CodeQualityRules", "HD1001:MethodMustNotHaveMoreThanFourParameters", Justification = "Needed to work with StyleCop CodeWalkerElementVisitor<T>")]
        bool Visit(Expression expression, Expression parentExpression, Statement parentStatement, CsElement parentElement, object context);
    }
}