namespace Cars.CodeQuality.Rules
{
    using System.Diagnostics.CodeAnalysis;
    using StyleCop.CSharp;

    public class NullExpressionVisitor : IExpressionVisitor
    {
        [SuppressMessage("Cars.CodeQuality.CodeQualityRules", "DV1001:MethodMustNotHaveMoreThanFourParameters", Justification = "Needed to work with StyleCop CodeWalkerElementVisitor<T>")]
        public bool Visit(
            Expression expression, 
            Expression parentExpression, 
            Statement parentStatement, 
            CsElement parentElement, 
            object context)
        {
            return true;
        }
    }
}