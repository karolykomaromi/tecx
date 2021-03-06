namespace TecX.Query.Visitors
{
    using System.Linq.Expressions;

    public class ContainsAlwaysTrueNode : ExpressionVisitor
    {
        public bool Found { get; private set; }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Value is bool)
            {
                if ((bool) node.Value)
                {
                    this.Found = true;
                }
            }

            return base.VisitConstant(node);
        }
    }
}