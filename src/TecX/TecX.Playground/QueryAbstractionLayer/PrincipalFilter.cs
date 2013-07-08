namespace TecX.Playground.QueryAbstractionLayer
{
    using System;
    using System.Linq.Expressions;

    public abstract class PrincipalFilter
    {
        public static readonly PrincipalFilter Include = new IncludePrincipalFilter();

        public static readonly PrincipalFilter Exclude = new ExcludePrincipalFilter();

        public abstract Expression<Func<TElement, bool>> Filter<TElement>()
            where TElement : PersistentObject;

        private class ExcludePrincipalFilter : PrincipalFilter
        {
            public override Expression<Func<TElement, bool>> Filter<TElement>()
            {
                ConstantExpression @true = Expression.Constant(true, typeof(bool));

                ParameterExpression p = Expression.Parameter(typeof(TElement), "p");

                Expression<Func<TElement, bool>> filter = Expression.Lambda<Func<TElement, bool>>(@true, p);

                return filter;
            }
        }

        private class IncludePrincipalFilter : PrincipalFilter
        {
            public override Expression<Func<TElement, bool>> Filter<TElement>()
            {
                ParameterExpression p = Expression.Parameter(typeof(TElement), "p");

                MemberExpression id = Expression.Property(p, "PrincipalId");

                long l = 1337;

                ConstantExpression leet = Expression.Constant(l, typeof(long));

                BinaryExpression equals = Expression.Equal(id, leet);

                Expression<Func<TElement, bool>> filter = Expression.Lambda<Func<TElement, bool>>(equals, p);

                return filter;
            }
        }
    }
}