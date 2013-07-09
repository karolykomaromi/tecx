using System.Linq.Expressions;

using TecX.Common;
using TecX.Playground.QueryAbstractionLayer.PD;
using TecX.Playground.QueryAbstractionLayer.Utility;

namespace TecX.Playground.QueryAbstractionLayer.Visitors
{
    public class AppendFrameworkFilters<TElement> : ExpressionVisitor
        where TElement : PersistentObject
    {
        private readonly PDIteratorOperator pdOperator;

        public AppendFrameworkFilters(PDIteratorOperator pdOperator)
        {
            Guard.AssertNotNull(pdOperator, "pdOperator");

            this.pdOperator = pdOperator;
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            // TODO weberse 2013-07-08 will never be called if there is no Where() condition in queryable
            node = ExpressionHelper.AppendOperatorFilters<T, TElement>(node, pdOperator);

            return base.VisitLambda<T>(node);
        }
    }
}