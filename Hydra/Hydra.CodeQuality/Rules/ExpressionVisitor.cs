namespace Hydra.CodeQuality.Rules
{
    using StyleCop;
    using StyleCop.CSharp;

    public abstract class ExpressionVisitor : Visitor, IExpressionVisitor
    {
        public static readonly IExpressionVisitor Null = new NullExpressionVisitor();

        protected ExpressionVisitor(SourceAnalyzer sourceAnalyzer)
            : base(sourceAnalyzer)
        {
        }

        public abstract bool Visit(Expression expression, Expression parentExpression, Statement parentStatement, CsElement parentElement, object context);
    }
}