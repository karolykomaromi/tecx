namespace TecX.Caching.QueryInterception
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;

    using TecX.Common;

    public class QueryInterceptor<T> : QueryInterceptor, IQueryable<T>
    {
        public QueryInterceptor(IQueryable<T> wrapped)
            : this(wrapped, new QueryInterceptorProvider(wrapped.Provider))
        {
        }

        public QueryInterceptor(IQueryable<T> wrapped, QueryInterceptorProvider provider)
            : base(wrapped, provider)
        {
        }

        public IEnumerator<T> GetEnumerator()
        {
            var enumerable = this.Provider.Execute<IEnumerable<T>>(this.Expression);

            var enumerator = enumerable.GetEnumerator();

            return enumerator;
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Reviewed. Suppression is OK here.")]
    public class QueryInterceptor : IQueryable
    {
        private readonly IQueryable wrapped;

        private readonly QueryInterceptorProvider queryProvider;

        public QueryInterceptor(IQueryable wrapped, QueryInterceptorProvider provider)
        {
            Guard.AssertNotNull(wrapped, "wrapped");
            Guard.AssertNotNull(provider, "provider");

            this.wrapped = wrapped;
            this.queryProvider = provider;
        }

        public Type ElementType
        {
            get
            {
                return this.wrapped.ElementType;
            }
        }

        public Expression Expression
        {
            get
            {
                return this.wrapped.Expression;
            }
        }

        public IQueryProvider Provider
        {
            get
            {
                return this.queryProvider;
            }
        }

        public QueryInterceptorProvider QueryProvider
        {
            get
            {
                return this.queryProvider;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            var enumerable = (IEnumerable)this.Provider.Execute(this.Expression);

            var enumerator = enumerable.GetEnumerator();

            return enumerator;
        }
    }
}