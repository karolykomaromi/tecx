using System;
using System.Linq.Expressions;

using TecX.Playground.QueryAbstractionLayer.PD;
using TecX.Playground.QueryAbstractionLayer.Utility;

namespace TecX.Playground.QueryAbstractionLayer.Filters
{
    public abstract class PrincipalFilter
    {
        public static readonly PrincipalFilter Enabled = new EnablePrincipalFilter();

        public static readonly PrincipalFilter Disabled = new DisablePrincipalFilter();

        public abstract Expression<Func<TElement, bool>> Filter<TElement>()
            where TElement : PersistentObject;

        private class DisablePrincipalFilter : PrincipalFilter
        {
            public override Expression<Func<TElement, bool>> Filter<TElement>()
            {
                return ExpressionHelper.AlwaysTrue<TElement>();
            }
        }

        private class EnablePrincipalFilter : PrincipalFilter
        {
            public override Expression<Func<TElement, bool>> Filter<TElement>()
            {
                ParameterExpression p = Expression.Parameter(typeof(TElement), "p");

                MemberExpression id = Expression.Property(p, "PrincipalId");

                // TODO weberse 2013-07-09 use current principal id instead of hardcoded value
                long l = 1337;

                ConstantExpression leet = Expression.Constant(l, typeof(long));

                BinaryExpression equals = Expression.Equal(id, leet);

                Expression<Func<TElement, bool>> filter = Expression.Lambda<Func<TElement, bool>>(equals, p);

                return filter;
            }
        }
    }
}