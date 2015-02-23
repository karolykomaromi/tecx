namespace Hydra.CodeQuality.Rules
{
    using StyleCop.CSharp;

    public class NullExpressionVisitor : IExpressionVisitor
    {
        public bool Visit(Expression expression, Expression parentExpression, Statement parentStatement, CsElement parentElement, object context)
        {
            return true;
        }
    }
}