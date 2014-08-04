namespace TecX.Caching.QueryInterception
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using TecX.Caching.KeyGeneration;
    using TecX.Common;

    public class QueryInterceptorProvider : IQueryProvider
    {
        private readonly IQueryProvider wrapped;

        public QueryInterceptorProvider(IQueryProvider wrapped)
        {
            Guard.AssertNotNull(wrapped, "wrapped");

            this.wrapped = wrapped;
        }

        public event EventHandler<ExpressionExecuteEventArgs> Executing = delegate { };

        public event EventHandler<ExpressionExecuteEventArgs> Executed = delegate { };

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            Guard.AssertNotNull(expression, "expression");

            var rawQuery = this.wrapped.CreateQuery<TElement>(expression);

            var interceptor = new QueryInterceptor<TElement>(rawQuery, this);

            return interceptor;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            Guard.AssertNotNull(expression, "expression");

            var rawQuery = this.wrapped.CreateQuery(expression);

            var interceptor = new QueryInterceptor(rawQuery, this);

            return interceptor;
        }

        public TResult Execute<TResult>(Expression expression)
        {
            Guard.AssertNotNull(expression, "expression");

            string cacheKey = expression.GetCacheKey();

            object value;
            bool handled = this.NotifyExecuting(expression, cacheKey, out value);

            TResult result = !handled ? this.wrapped.Execute<TResult>(expression) : (TResult)value;

            this.NotifyExecuted(expression, cacheKey, result);

            return result;
        }

        public object Execute(Expression expression)
        {
            Guard.AssertNotNull(expression, "expression");

            string cacheKey = expression.GetCacheKey();

            object value;
            bool handled = this.NotifyExecuting(expression, cacheKey, out value);

            object result = !handled ? this.wrapped.Execute(expression) : value;

            this.NotifyExecuted(expression, cacheKey, result);

            return result;
        }

        private bool NotifyExecuting(Expression expression, string cacheKey, out object result)
        {
            Guard.AssertNotNull(expression, "expression");
            Guard.AssertNotEmpty(cacheKey, "cacheKey");

            var e = new ExpressionExecuteEventArgs
                {
                    Expression = expression, 
                    CacheKey = cacheKey
                };

            this.Executing(this, e);

            if (e.Handled)
            {
                result = e.Result;
                return true;
            }

            result = null;
            return false;
        }

        private void NotifyExecuted(Expression expression, string cacheKey, object result)
        {
            Guard.AssertNotNull(expression, "expression");
            Guard.AssertNotEmpty(cacheKey, "cacheKey");

            var e = new ExpressionExecuteEventArgs
                {
                    Expression = expression,
                    CacheKey = cacheKey,
                    Result = result
                };

            this.Executed(this, e);
        }
    }
}