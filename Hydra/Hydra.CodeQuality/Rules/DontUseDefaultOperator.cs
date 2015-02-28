namespace Hydra.CodeQuality.Rules
{
    using System.Diagnostics.CodeAnalysis;
    using StyleCop;
    using StyleCop.CSharp;

    public class DontUseDefaultOperator : ExpressionVisitor
    {
        public DontUseDefaultOperator(SourceAnalyzer sourceAnalyzer)
            : base(sourceAnalyzer)
        {
        }

        [SuppressMessage("Hydra.CodeQuality.CodeQualityRules", "HD1001:MethodMustNotHaveMoreThanFourParameters", Justification = "Needed to work with StyleCop CodeWalkerElementVisitor<T>")]
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
