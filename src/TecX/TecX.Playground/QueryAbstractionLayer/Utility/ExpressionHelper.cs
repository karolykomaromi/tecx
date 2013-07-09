namespace TecX.Playground.QueryAbstractionLayer.Utility
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using TecX.Playground.QueryAbstractionLayer.PD;
    using TecX.Playground.QueryAbstractionLayer.Visitors;

    public static class ExpressionHelper
    {
        public static Expression<Func<TElement, bool>> AlwaysTrue<TElement>()
            where TElement : PersistentObject
        {
            ConstantExpression @true = Expression.Constant(true, typeof(bool));

            ParameterExpression p = Expression.Parameter(typeof(TElement), "p");

            Expression<Func<TElement, bool>> filter = Expression.Lambda<Func<TElement, bool>>(@true, p);

            return filter;
        }

        public static Expression<T> And<T>(this Expression<T> left, Expression<T> right)
        {
            var map = left.Parameters.Select((parameter, index) => new { Found = parameter, ReplaceWith = right.Parameters[index] }).ToDictionary(p => p.ReplaceWith, p => p.Found);

            var secondBody = new ParameterRebinder(map).Visit(right.Body);

            return Expression.Lambda<T>(Expression.And(left.Body, secondBody), left.Parameters);
        }

        public static Expression<T> AppendFiltersFromOperator<T, TElement>(Expression<T> node, PDIteratorOperator pdOperator, IClientInfo clientInfo)
            where TElement : PersistentObject
        {
            Expression<T> filter = pdOperator.PrincipalFilter.Filter<TElement>(clientInfo) as Expression<T>;

            if (filter != null)
            {
                node = node.And(filter);
            }

            filter = pdOperator.IncludeDeletedItems.Filter<TElement>() as Expression<T>;

            if (filter != null)
            {
                node = node.And(filter);
            }
            return node;
        }

        public static MemberExpression Property<TElement, TProperty>(Expression<Func<TElement, TProperty>> selector)
        {
            return (MemberExpression) selector.Body;
        }
    }
}