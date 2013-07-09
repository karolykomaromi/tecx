namespace TecX.Playground.QueryAbstractionLayer.Visitors
{
    using System.Linq.Expressions;

    using TecX.Common;
    using TecX.Playground.QueryAbstractionLayer.PD;
    using TecX.Playground.QueryAbstractionLayer.Utility;

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
            // TODO weberse 2013-07-08 will never be called if there is no Where() condition in queryable
            node = ExpressionHelper.AppendFiltersFromOperator<T, TElement>(node, pdOperator, clientInfo);

            return base.VisitLambda<T>(node);
        }
    }
}