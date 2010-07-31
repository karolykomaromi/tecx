using System;
using System.Linq;
using System.Linq.Expressions;

namespace TecX.Common.Extensions.LinqTo.Objects
{
    /// <summary>
    /// This class extends the generic type "Expression(Func(T, bool))" to easily concatenate 
    /// multiple expressions of this type for a where statement
    /// </summary>
    public static class PredicateExtensions
    {
        /// <summary>
        /// "always true" Expression for LinQ query building. This is a value you can start with to 
        /// build up a sequence of AND-concatenated expressions.
        /// </summary>
        /// <typeparam name="T">pass here the type of the object you want to use inside the excpression</typeparam>
        /// <returns>an expression that returns TRUE</returns>
        public static Expression<Func<T, bool>> True<T>()
        {
            return f => true;
        }

        /// <summary>
        /// "always false" Expression for LinQ query building. This is a value you can start with to 
        /// build up a sequence of OR-concatenated expressions.
        /// </summary>
        /// <typeparam name="T">pass here the type of the object you want to use inside the excpression</typeparam>
        /// <returns>an expression that returns FALSE</returns>
        public static Expression<Func<T, bool>> False<T>()
        {
            return f => false;
        }

        /// <summary>
        /// Concatenates two expressions using the OR operator resulting a new expression
        /// </summary>
        /// <typeparam name="T">the type of object you want to use inside the expression tree</typeparam>
        /// <param name="expression1">the "source" expression</param>
        /// <param name="expression2">the second expression to "add" to the first one using "OR"</param>
        /// <returns>a comined expression using OR</returns>
        public static Expression<Func<T, bool>> Or<T>(
            this Expression<Func<T, bool>> expression1,
            Expression<Func<T, bool>> expression2)
        {
            var invokedExpression = Expression.Invoke(expression2, expression1.Parameters.Cast<Expression>());

            return Expression.Lambda<Func<T, bool>>(
                Expression.Or(expression1.Body, invokedExpression),
                expression1.Parameters);
        }

        /// <summary>
        /// Concatenates two expressions using the AND operator resulting a new expression
        /// </summary>
        /// <typeparam name="T">the type of object you want to use inside the expression tree</typeparam>
        /// <param name="expression1">the "source" expression</param>
        /// <param name="expression2">the second expression to "add" to the first one using "AND"</param>
        /// <returns>a comined expression using AND</returns>
        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> expression1,
            Expression<Func<T, bool>> expression2)
        {
            var invokedExpression = Expression.Invoke(expression2, expression1.Parameters.Cast<Expression>());

            return Expression.Lambda<Func<T, bool>>(
                Expression.And(expression1.Body, invokedExpression),
                expression1.Parameters);
        }
    }
}