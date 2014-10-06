namespace TecX.Common.Extensions.LinqTo.Entities
{
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Walks an expression tree and replaces the call parameters so that a parameter that
    /// is used in the root of an expression tree is reused by all expressions further down the road
    /// </summary>
    /// <remarks>Extracted from article on 
    /// <see cref="http://blogs.msdn.com/b/meek/archive/2008/05/02/linq-to-entities-combining-predicates.aspx"/>
    /// </remarks>
    public class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;

            if (this.map.TryGetValue(p, out replacement))
            {
                p = replacement;
            }

            return base.VisitParameter(p);
        }
    }
}
