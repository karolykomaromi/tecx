namespace TecX.Query.Utility
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using TecX.Query.PD;
    using TecX.Query.Visitors;

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

        public static Func<PDIteratorOperator, IClientInfo, ExpressionVisitor> CreateAppendFrameworkFiltersVisitor(Type type)
        {
            ParameterExpression p1 = Expression.Parameter(typeof(PDIteratorOperator), "op");

            ParameterExpression p2 = Expression.Parameter(typeof(IClientInfo), "clientInfo");

            Type visitorType = typeof(AppendFrameworkFilters<>).MakeGenericType(type);

            ConstructorInfo ctor = visitorType.GetConstructor(new[] { typeof(PDIteratorOperator), typeof(IClientInfo) });

            NewExpression @new = Expression.New(ctor, p1, p2);

            Expression<Func<PDIteratorOperator, IClientInfo, ExpressionVisitor>> factoryExpression =
                Expression.Lambda<Func<PDIteratorOperator, IClientInfo, ExpressionVisitor>>(@new, p1, p2);

            Func<PDIteratorOperator, IClientInfo, ExpressionVisitor> factory = factoryExpression.Compile();

            return factory;
        }

        public static Func<PDIteratorOperator, IClientInfo, ExpressionVisitor> CreatePrependWhereClauseVisitor(Type type)
        {
            ParameterExpression p1 = Expression.Parameter(typeof(PDIteratorOperator), "op");

            ParameterExpression p2 = Expression.Parameter(typeof(IClientInfo), "clientInfo");

            Type visitorType = typeof(PrependWhereClause<>).MakeGenericType(type);

            ConstructorInfo ctor = visitorType.GetConstructor(new[] { typeof(PDIteratorOperator), typeof(IClientInfo) });

            NewExpression @new = Expression.New(ctor, p1, p2);

            Expression<Func<PDIteratorOperator, IClientInfo, ExpressionVisitor>> factoryExpression =
                Expression.Lambda<Func<PDIteratorOperator, IClientInfo, ExpressionVisitor>>(@new, p1, p2);

            Func<PDIteratorOperator, IClientInfo, ExpressionVisitor> factory = factoryExpression.Compile();

            return factory;
        }
    }
}