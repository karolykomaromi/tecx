namespace Hydra.CodeQuality.Rules
{
    using StyleCop.CSharp;

    public interface IExpressionVisitor
    {
        bool Visit(Expression expression, Expression parentExpression, Statement parentStatement, CsElement parentElement, object context);
    }
}