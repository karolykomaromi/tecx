namespace TecX.Playground.QueryAbstractionLayer
{
    using System.Linq;
    using System.Linq.Expressions;

    public static class ExpressionExtensions
    {
        public static Expression<T> And<T>(this Expression<T> left, Expression<T> right)
        {
            var map = left.Parameters.Select((parameter, index) => new { Found = parameter, ReplaceWith = right.Parameters[index] }).ToDictionary(p => p.ReplaceWith, p => p.Found);

            var secondBody = new ParameterRebinder(map).Visit(right.Body);

            return Expression.Lambda<T>(Expression.And(left.Body, secondBody), left.Parameters);
        }
    }
}