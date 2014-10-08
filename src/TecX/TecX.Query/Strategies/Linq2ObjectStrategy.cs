namespace TecX.Query.Strategies
{
    using System;
    using System.Linq.Expressions;

    using TecX.Common;
    using TecX.Query.PD;
    using TecX.Query.Visitors;

    public class Linq2ObjectStrategy : ExpressionManipulationStrategy
    {
        private readonly PDIteratorOperator pdOperator;
        private readonly IClientInfo clientInfo;

        private readonly VisitorCache appendFrameworkFiltersVisitors;
        private readonly VisitorCache prependWhereClauseVisitors;

        public Linq2ObjectStrategy(PDIteratorOperator pdOperator, IClientInfo clientInfo, VisitorCache frameworkFilter, VisitorCache whereClauses)
        {
            Guard.AssertNotNull(pdOperator, "pdOperator");
            Guard.AssertNotNull(clientInfo, "clientInfo");
            Guard.AssertNotNull(frameworkFilter, "frameworkFilter");
            Guard.AssertNotNull(whereClauses, "whereClauses");

            this.pdOperator = pdOperator;
            this.clientInfo = clientInfo;
            this.appendFrameworkFiltersVisitors = frameworkFilter;
            this.prependWhereClauseVisitors = whereClauses;
        }

        public override Expression Process(Expression expression, Type elementType)
        {
            Guard.AssertNotNull(expression, "expression");
            Guard.AssertNotNull(elementType, "elementType");

            Expression newExpression = expression;

            ExpressionVisitor visitor;
            if (this.appendFrameworkFiltersVisitors.TryGetVisitor(elementType, this.pdOperator, this.clientInfo, out visitor))
            {
                newExpression = visitor.Visit(expression);
            }

            if (newExpression == expression)
            {
                if (this.prependWhereClauseVisitors.TryGetVisitor(elementType, this.pdOperator, this.clientInfo, out visitor))
                {
                    newExpression = visitor.Visit(expression);
                }
            }

            return newExpression ?? Expression.Empty();
        }
    }
}