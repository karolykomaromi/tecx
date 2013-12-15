namespace TecX.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using TecX.Common;
    using TecX.Query.Strategies;
    using TecX.Query.Visitors;

    public class QueryProviderInterceptor : IQueryProvider
    {
        private readonly IQueryProvider inner;
        private readonly ExpressionManipulationStrategy strategy;

        public QueryProviderInterceptor(IQueryProvider inner, ExpressionManipulationStrategy strategy)
        {
            Guard.AssertNotNull(inner, "inner");
            Guard.AssertNotNull(strategy, "strategy");

            this.inner = inner;
            this.strategy = strategy;
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
                throw new NotImplementedException("Could not identify type of elements in IQueryable.");
            }

            Expression newExpression = this.strategy.Process(expression, finder.ElementType);

            ExpressionVisitor cleanup = new CleanupAlwaysTrueNodes();

            newExpression = cleanup.Visit(newExpression);

            return this.inner.Execute<TResult>(newExpression);
        }

        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }
    }
}