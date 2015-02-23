namespace Hydra.CodeQuality.Rules
{
    using StyleCop;
    using StyleCop.CSharp;

    public class DontUseDefaultOperator : ExpressionVisitor
    {
        public DontUseDefaultOperator(SourceAnalyzer sourceAnalyzer)
            : base(sourceAnalyzer)
        {
        }

        public override bool Visit(Expression expression, Expression parentExpression, Statement parentStatement, CsElement parentElement, object context)
        {
            DefaultValueExpression defaultValueExpression = expression as DefaultValueExpression;

            if (defaultValueExpression != null)
            {
                this.SourceAnalyzer.AddViolation(
                    parentElement,
                    defaultValueExpression.Location,
                    this.RuleName,
                    defaultValueExpression.Type);
            }

            return true;
        }
    }
}
