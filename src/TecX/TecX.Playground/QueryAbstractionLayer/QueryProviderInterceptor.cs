namespace TecX.Playground.QueryAbstractionLayer
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using TecX.Common;

    public class QueryProviderInterceptor : IQueryProvider
    {
        private readonly IQueryProvider inner;

        private readonly PDOperator pdOperator;

        private readonly VisitorCache visitorCache;

        public QueryProviderInterceptor(IQueryProvider inner, PDOperator pdOperator)
        {
            Guard.AssertNotNull(inner, "inner");
            Guard.AssertNotNull(pdOperator, "pdOperator");

            this.inner = inner;
            this.pdOperator = pdOperator;
            this.visitorCache = new VisitorCache();
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            IQueryable<TElement> raw = this.inner.CreateQuery<TElement>(expression);

            return new QueryInterceptor<TElement>(raw, this);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            FindElementType finder = new FindElementType();

            finder.Visit(expression);

            if (finder.ElementType == null)
            {
                // TODO weberse 2013-07-08 no lamdba in expression ...
            }

            Expression newExpression = expression;

            ExpressionVisitor visitor;
            if (this.visitorCache.TryGetVisitor(finder.ElementType, this.pdOperator, out visitor))
            {
                newExpression = visitor.Visit(expression);
            }

            if (newExpression == expression)
            {
                // TODO weberse 2013-07-08 no lamdba in expression we could add to :(
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