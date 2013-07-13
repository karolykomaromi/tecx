namespace TecX.Query.Visitors
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using TecX.Common;
    using TecX.Query.PD;
    using TecX.Query.Utility;

    public class PrependWhereClause<TElement> : ExpressionVisitor
        where TElement : PersistentObject
    {
        private readonly PDIteratorOperator pdOperator;

        private readonly IClientInfo clientInfo;

        private readonly Expression<Func<IQueryable<TElement>, IQueryable<TElement>>> whereClause = elements => elements.Where(e => e != null);

        public PrependWhereClause(PDIteratorOperator pdOperator, IClientInfo clientInfo)
        {
            Guard.AssertNotNull(pdOperator, "pdOperator");
            Guard.AssertNotNull(clientInfo, "clientInfo");

            this.pdOperator = pdOperator;
            this.clientInfo = clientInfo;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (!string.Equals(node.Method.Name, "Where", StringComparison.OrdinalIgnoreCase))
            {
                MethodCallExpression callToWhere = (MethodCallExpression)this.whereClause.Body;

                Expression<Func<TElement, bool>> systemFilters = ExpressionHelper.AppendFiltersFromOperator<Func<TElement, bool>, TElement>(
                        ExpressionHelper.AlwaysTrue<TElement>(), 
                        this.pdOperator, 
                        this.clientInfo);

                node = Expression.Call(node.Method, Expression.Call(callToWhere.Method, node.Arguments[0], systemFilters));
            }

            return base.VisitMethodCall(node);
        }
    }
}