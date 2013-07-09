using System;
using System.Linq.Expressions;

using TecX.Playground.QueryAbstractionLayer.PD;
using TecX.Playground.QueryAbstractionLayer.Utility;

namespace TecX.Playground.QueryAbstractionLayer.Filters
{
    public abstract class DeletedItemsFilter
    {
        public static readonly DeletedItemsFilter Include = new IncludeDeletedItemsFilter();

        public static readonly DeletedItemsFilter Exclude = new ExcludeDeletedItemsFilter();

        public abstract Expression<Func<TElement, bool>> Filter<TElement>()
            where TElement : PersistentObject;

        private class IncludeDeletedItemsFilter : DeletedItemsFilter
        {
            public override Expression<Func<TElement, bool>> Filter<TElement>()
            {
                return ExpressionHelper.AlwaysTrue<TElement>();
            }
        }

        private class ExcludeDeletedItemsFilter : DeletedItemsFilter
        {
            public override Expression<Func<TElement, bool>> Filter<TElement>()
            {
                ParameterExpression p = Expression.Parameter(typeof (TElement), "p");

                MemberExpression isDeleted = Expression.Property(p, "IsDeleted");

                ConstantExpression @false = Expression.Constant(false, typeof (bool));

                BinaryExpression equals = Expression.Equal(isDeleted, @false);

                Expression<Func<TElement, bool>> filter = Expression.Lambda<Func<TElement, bool>>(equals, p);

                return filter;
            }
        }
    }
}