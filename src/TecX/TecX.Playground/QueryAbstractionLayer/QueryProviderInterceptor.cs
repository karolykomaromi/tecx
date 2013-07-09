namespace TecX.Playground.QueryAbstractionLayer
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using TecX.Common;
    using TecX.Playground.QueryAbstractionLayer.PD;
    using TecX.Playground.QueryAbstractionLayer.Utility;
    using TecX.Playground.QueryAbstractionLayer.Visitors;

    public class QueryProviderInterceptor : IQueryProvider
    {
        private readonly IQueryProvider inner;

        private readonly PDIteratorOperator pdOperator;
        private readonly IClientInfo clientInfo;

        private readonly VisitorCache appendFrameworkFiltersVisitors;
        private readonly VisitorCache prependWhereClauseVisitors;

        public QueryProviderInterceptor(IQueryProvider inner, PDIteratorOperator pdOperator, IClientInfo clientInfo)
        {
            Guard.AssertNotNull(inner, "inner");
            Guard.AssertNotNull(pdOperator, "pdOperator");
            Guard.AssertNotNull(clientInfo, "clientInfo");

            this.inner = inner;
            this.pdOperator = pdOperator;
            this.clientInfo = clientInfo;
            this.appendFrameworkFiltersVisitors = new VisitorCache(ExpressionHelper.CreateAppendFrameworkFiltersVisitor);
            this.prependWhereClauseVisitors = new VisitorCache(ExpressionHelper.CreatePrependWhereClauseVisitor);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            IQueryable<TElement> raw = this.inner.CreateQuery<TElement>(expression);

            return new QueryInterceptor<TElement>(raw, this);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            ElementTypeFinder finder = new ElementTypeFinder();

            finder.Visit(expression);

            if (finder.ElementType == null)
            {
                // TODO weberse 2013-07-08 couldn't identify the element type :(
                throw new NotImplementedException("Could not identify type of elements in IQueryable.");
            }

            Expression newExpression = expression;

            ExpressionVisitor visitor;
            if (this.appendFrameworkFiltersVisitors.TryGetVisitor(finder.ElementType, this.pdOperator, this.clientInfo, out visitor))
            {
                newExpression = visitor.Visit(expression);
            }

            if (newExpression == expression)
            {
                if (this.prependWhereClauseVisitors.TryGetVisitor(finder.ElementType, this.pdOperator, this.clientInfo, out visitor))
                {
                    newExpression = visitor.Visit(expression);
                }
            }

            return this.inner.Execute<TResult>(newExpression);
        }

        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        public object Execute(Expression expression)
        {
            // TODO weberse 2013-07-08 we can get the element type from the expression node so we can also use non-generic methods here!
            throw new NotImplementedException();
        }
    }
}