namespace TecX.Query.Visitors
{
    using System.Linq.Expressions;

    using TecX.Common;
    using TecX.Query.PD;
    using TecX.Query.Utility;

    public class AppendFrameworkFilters<TElement> : ExpressionVisitor
        where TElement : PersistentObject
    {
        private readonly PDIteratorOperator pdOperator;
        private readonly IClientInfo clientInfo;

        public AppendFrameworkFilters(PDIteratorOperator pdOperator, IClientInfo clientInfo)
        {
            Guard.AssertNotNull(pdOperator, "pdOperator");
            Guard.AssertNotNull(clientInfo, "clientInfo");

            this.pdOperator = pdOperator;
            this.clientInfo = clientInfo;
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            MemberExpression member = node.Body as MemberExpression;

            if (member == null)
            {
                node = ExpressionHelper.AppendFiltersFromOperator<T, TElement>(node, this.pdOperator, this.clientInfo);
            }

            return base.VisitLambda<T>(node);
        }
    }
}