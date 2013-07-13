namespace TecX.Query.Visitors
{
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public class ParameterRebinder : ExpressionVisitor
    {
        private readonly IDictionary<ParameterExpression, ParameterExpression> map;

        public ParameterRebinder(IDictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
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