namespace TecX.Playground.QueryAbstractionLayer
{
    using System.Linq.Expressions;

    using TecX.Common;

    public class ConcatFrameworkFilters<TElement> : ExpressionVisitor
        where TElement : PersistentObject
    {
        private readonly PDOperator pdOperator;

        public ConcatFrameworkFilters(PDOperator pdOperator)
        {
            Guard.AssertNotNull(pdOperator, "pdOperator");
            this.pdOperator = pdOperator;
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            // TODO weberse 2013-07-08 will never be called if there is no Where() condition in queryable
            Expression<T> filter = this.pdOperator.IncludePrincipalFilter.Filter<TElement>() as Expression<T>;

            if (filter != null)
            {
                node = node.And(filter);
            }

            return base.VisitLambda<T>(node);
        }
    }
}