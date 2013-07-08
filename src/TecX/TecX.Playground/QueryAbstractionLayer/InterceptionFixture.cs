using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TecX.Playground.QueryAbstractionLayer
{
    using System.Collections;
    using System.Linq.Expressions;

    using TecX.Common;

    using Xunit;

    public class InterceptionFixture
    {
        [Fact]
        public void Should_AlwaysBeTrue()
        {
            Assert.True(PrincipalFilter.Exclude.Filter<Func<Foo, bool>>().Compile()(new Foo()));
        }

        [Fact]
        public void Should_BeTrueOnLeet()
        {
            Assert.True(PrincipalFilter.Include.Filter<Func<Foo, bool>>().Compile()(new Foo { Id = 1337 }));
        }

        [Fact]
        public void Should_Wrap()
        {
            IQueryable<Foo> query = new[] { new Foo { Id = 1 }, new Foo { Id = 2 }, new Foo { Id = 1337 } }.AsQueryable();

            IQueryable<Foo> intercepted = new QueryInterceptor<Foo>(query, new PDOperator());
            //IQueryable<Foo> intercepted = new QueryInterceptor<Foo>(query, new PDOperator()).Where(f => string.IsNullOrEmpty(f.Bar));

            Assert.Equal(1, intercepted.Count());
        }
    }

    public class Foo : SomeBaseClass
    {
        public string Bar { get; set; }
    }

    public class QueryInterceptor<TElement> : IQueryable<TElement>
    {
        private readonly IQueryable<TElement> inner;

        private readonly QueryProviderInterceptor provider;

        public QueryInterceptor(IQueryable<TElement> inner, PDOperator pdOperator)
            : this(inner, new QueryProviderInterceptor(inner.Provider, pdOperator))
        {
        }

        public QueryInterceptor(IQueryable<TElement> inner, QueryProviderInterceptor provider)
        {
            Guard.AssertNotNull(inner, "inner");
            Guard.AssertNotNull(provider, "provider");

            this.inner = inner;
            this.provider = provider;
        }

        public Expression Expression { get { return inner.Expression; } }

        public Type ElementType { get { return inner.ElementType; } }

        public IQueryProvider Provider { get { return provider; } }

        public IEnumerator<TElement> GetEnumerator()
        {
            return this.provider.Execute<IEnumerable<TElement>>(this.inner.Expression).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public class PDOperator
    {
        public PDOperator()
        {
            this.IncludePrincipalFilter = PrincipalFilter.Include;
        }

        public PrincipalFilter IncludePrincipalFilter { get; set; }
    }

    public abstract class PrincipalFilter
    {
        public static readonly PrincipalFilter Include = new IncludePrincipalFilter();

        public static readonly PrincipalFilter Exclude = new ExcludePrincipalFilter();

        public abstract Expression<T> Filter<T>();

        private class ExcludePrincipalFilter : PrincipalFilter
        {
            public override Expression<T> Filter<T>()
            {
                ConstantExpression @true = Expression.Constant(true, typeof(bool));

                ParameterExpression p = Expression.Parameter(typeof(T).GetGenericArguments()[0], "p");

                Expression<T> filter = Expression.Lambda<T>(@true, p);

                return filter;
            }
        }

        private class IncludePrincipalFilter : PrincipalFilter
        {
            public override Expression<T> Filter<T>()
            {
                ParameterExpression p = Expression.Parameter(typeof(T).GetGenericArguments()[0], "p");

                MemberExpression id = Expression.Property(p, "Id");

                long l = 1337;

                ConstantExpression leet = Expression.Constant(l, typeof(long));

                BinaryExpression equals = Expression.Equal(id, leet);

                Expression<T> filter = Expression.Lambda<T>(equals, p);

                return filter;
            }
        }
    }

    public class SomeBaseClass
    {
        public long Id { get; set; }
    }

    public class QueryProviderInterceptor : IQueryProvider
    {
        private readonly IQueryProvider inner;

        private readonly PDOperator pdOperator;

        public QueryProviderInterceptor(IQueryProvider inner, PDOperator pdOperator)
        {
            Guard.AssertNotNull(inner, "inner");
            Guard.AssertNotNull(pdOperator, "pdOperator");

            this.inner = inner;
            this.pdOperator = pdOperator;
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            IQueryable<TElement> raw = inner.CreateQuery<TElement>(expression);

            return new QueryInterceptor<TElement>(raw, this);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            Expression newExpression = new ConcatFrameworkFilters(pdOperator).Visit(expression);

            if (newExpression == expression)
            {
                
            }

            return inner.Execute<TResult>(newExpression);
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

    public static class ExpressionExtensions
    {
        public static Expression<T> And<T>(this Expression<T> left, Expression<T> right)
        {
            var map = left.Parameters.Select((parameter, index) => new { Found = parameter, ReplaceWith = right.Parameters[index] }).ToDictionary(p => p.ReplaceWith, p => p.Found);

            var secondBody = new ParameterRebinder(map).Visit(right.Body);

            return Expression.Lambda<T>(Expression.And(left.Body, secondBody), left.Parameters);
        }
    }

    public class ConcatFrameworkFilters : ExpressionVisitor
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
            Expression<T> filter = this.pdOperator.IncludePrincipalFilter.Filter<T>();

            node = node.And(filter);

            return base.VisitLambda<T>(node);
        }
    }

    public class ParameterRebinder : ExpressionVisitor
    {
        private readonly IDictionary<ParameterExpression, ParameterExpression> map;

        public ParameterRebinder(IDictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;

            if (this.map.TryGetValue(p, out replacement))
            {
                p = replacement;
            }

            return base.VisitParameter(p);
        }
    }
}
