namespace TecX.Playground.QueryAbstractionLayer
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using TecX.Common;
    using TecX.Playground.QueryAbstractionLayer.PD;

    public class QueryInterceptor<TElement> : IQueryable<TElement>
    {
        private readonly IQueryable<TElement> inner;

        private readonly QueryProviderInterceptor provider;

        public QueryInterceptor(IQueryable<TElement> inner, PDIteratorOperator pdOperator, IClientInfo clientInfo)
            : this(inner, new QueryProviderInterceptor(inner.Provider, pdOperator, clientInfo))
        {
        }

        public QueryInterceptor(IQueryable<TElement> inner, QueryProviderInterceptor provider)
        {
            Guard.AssertNotNull(inner, "inner");
            Guard.AssertNotNull(provider, "provider");

            this.inner = inner;
            this.provider = provider;
        }

        public Expression Expression { get { return this.inner.Expression; } }

        public Type ElementType { get { return this.inner.ElementType; } }

        public IQueryProvider Provider { get { return this.provider; } }

        public IEnumerator<TElement> GetEnumerator()
        {
            return this.provider.Execute<IEnumerable<TElement>>(this.inner.Expression).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}