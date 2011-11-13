namespace TecX.Common.Extensions.LinqTo.Entities
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// This class extends the generic type "Expression(Func(T, bool))" to easily concatenate 
    /// multiple expressions of this type for a where statement
    /// </summary>
    /// <remarks>Performs parameter rebinding using the <see cref="ParameterRebinder"/> which
    /// in turns is derived from <see cref="ExpressionVisitor"/>. This class is extracted from
    /// http://blogs.msdn.com/b/meek/archive/2008/05/02/linq-to-entities-combining-predicates.aspx
    /// </remarks>
    public static class PredicateExtensions
    {
        /// <summary>
        /// "always true" Expression for LinQ query building. This is a value you can start with to 
        /// build up a sequence of AND-concatenated expressions.
        /// </summary>
        /// <typeparam name="T">pass here the type of the object you want to use inside the excpression</typeparam>
        /// <returns>an expression that returns <c>true</c></returns>
        public static Expression<Func<T, bool>> True<T>()
        {
            return t => true;
        }

        /// <summary>
        /// "always false" Expression for LinQ query building. This is a value you can start with to 
        /// build up a sequence of OR-concatenated expressions.
        /// </summary>
        /// <typeparam name="T">pass here the type of the object you want to use inside the excpression</typeparam>
        /// <returns>an expression that returns <c>false</c></returns>
        public static Expression<Func<T, bool>> False<T>()
        {
            return f => false;
        }

        public static Expression<T> Compose<T>(
            this Expression<T> left, 
            Expression<T> right, 
            Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of right to parameters of left)
            // TODO weberse might be helpful if you don't name the variables f,i,s and p...
            var map = left.Parameters.Select((f, i) => new { f, s = right.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

            // replace parameters in the right lambda expression with parameters from the left
            var secondBody = ParameterRebinder.ReplaceParameters(map, right.Body);

            // apply composition of lambda expression bodies to parameters from the left expression 
            return Expression.Lambda<T>(merge(left.Body, secondBody), left.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            return left.Compose(right, Expression.And);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            return left.Compose(right, Expression.Or);
        }
    }
}
