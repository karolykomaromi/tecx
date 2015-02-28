namespace Hydra.CodeQuality.Rules
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using StyleCop.CSharp;

    public class CompositeExpressionVisitor : IExpressionVisitor
    {
        private readonly List<IExpressionVisitor> visitors;

        public CompositeExpressionVisitor(params IExpressionVisitor[] visitors)
        {
            this.visitors = new List<IExpressionVisitor>(visitors ?? new IExpressionVisitor[0]);
        }

        [SuppressMessage("Hydra.CodeQuality.CodeQualityRules", "HD1001:MethodMustNotHaveMoreThanFourParameters", Justification = "Needed to work with StyleCop CodeWalkerElementVisitor<T>")]
        public bool Visit(
            Expression expression, 
            Expression parentExpression, 
            Statement parentStatement, 
            CsElement parentElement, 
            object context)
        {
            bool b = true;
            
            this.visitors.ForEach(visitor => b &= visitor.Visit(
                expression, 
                parentExpression, 
                parentStatement, 
                parentElement, 
                context));

            return b;
        }
    }
}