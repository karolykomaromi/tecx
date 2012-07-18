namespace TecX.Common.Extensions.LinqTo.Objects
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// This class extends the generic type "Expression(Func(T, bool))" to easily concatenate 
    /// multiple expressions of this type for a where statement
    /// </summary>
    /// <remarks>Uses <see cref="InvocationExpression"/> which is not supported by the Entity Framework
    /// but works quite well with regular .NET queries</remarks>
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

        /// <summary>
        /// Concatenates two expressions using the OR operator resulting a new expression
        /// </summary>
        /// <typeparam name="T">the type of object you want to use inside the expression tree</typeparam>
        /// <param name="left">the "source" expression</param>
        /// <param name="right">the second expression to "add" to the first one using "OR"</param>
        /// <returns>a comined expression using OR</returns>
        public static Expression<Func<T, bool>> Or<T>(
            this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            Guard.AssertNotNull(left, "left");
            Guard.AssertNotNull(right, "right");

            var invokedExpression = Expression.Invoke(right, left.Parameters.Cast<Expression>());

            return Expression.Lambda<Func<T, bool>>(
                Expression.Or(left.Body, invokedExpression),
                left.Parameters);
        }

        /// <summary>
        /// Concatenates two expressions using the AND operator resulting a new expression
        /// </summary>
        /// <typeparam name="T">the type of object you want to use inside the expression tree</typeparam>
        /// <param name="left">the "source" expression</param>
        /// <param name="right">the second expression to "add" to the first one using "AND"</param>
        /// <returns>a comined expression using AND</returns>
        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            Guard.AssertNotNull(left, "left");
            Guard.AssertNotNull(right, "right");

            var invokedExpression = Expression.Invoke(right, left.Parameters.Cast<Expression>());

            return Expression.Lambda<Func<T, bool>>(
                Expression.And(left.Body, invokedExpression),
                left.Parameters);
        }
    }
}