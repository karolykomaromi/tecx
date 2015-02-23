namespace Hydra.CodeQuality.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using StyleCop.CSharp;

    public class CompositeExpressionVisitor : IExpressionVisitor
    {
        private readonly List<IExpressionVisitor> visitors;

        public CompositeExpressionVisitor(params IExpressionVisitor[] visitors)
        {
            this.visitors = new List<IExpressionVisitor>(visitors ?? new IExpressionVisitor[0]);
        }

        public bool Visit(Expression expression, Expression parentExpression, Statement parentStatement, CsElement parentElement, object context)
        {
            return this.visitors.All(visitor => visitor.Visit(expression, parentExpression, parentStatement, parentElement, context));
        }
    }
}