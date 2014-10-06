namespace TecX.Query.Filters
{
    using System;
    using System.Linq.Expressions;

    using TecX.Query.PD;
    using TecX.Query.Utility;

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
                ParameterExpression p = Expression.Parameter(typeof(TElement), "p");

                MemberExpression pdoDeleted = Expression.Property(p, "PDO_DELETED");

                ConstantExpression @null = Expression.Constant(null, typeof(DateTime?));

                BinaryExpression equals = Expression.Equal(pdoDeleted, @null);

                Expression<Func<TElement, bool>> filter = Expression.Lambda<Func<TElement, bool>>(equals, p);

                return filter;
            }
        }
    }
}