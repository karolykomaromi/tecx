namespace Cars.CodeQuality.Rules
{
    using System.Diagnostics.CodeAnalysis;
    using StyleCop;
    using StyleCop.CSharp;

    public abstract class ExpressionVisitor : Visitor, IExpressionVisitor
    {
        public static readonly IExpressionVisitor Null = new NullExpressionVisitor();

        protected ExpressionVisitor(SourceAnalyzer sourceAnalyzer)
            : base(sourceAnalyzer)
        {
        }

        [SuppressMessage("Cars.CodeQuality.CodeQualityRules", "DV1001:MethodMustNotHaveMoreThanFourParameters", Justification = "Needed to work with StyleCop CodeWalkerElementVisitor<T>")]
        public abstract bool Visit(Expression expression, Expression parentExpression, Statement parentStatement, CsElement parentElement, object context);
    }
}