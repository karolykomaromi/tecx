namespace TecX.Query.Visitors
{
    using System.Linq.Expressions;

    public class CleanupAlwaysTrueNodes : ExpressionVisitor
    {
        protected override Expression VisitBinary(BinaryExpression node)
        {
            ConstantExpression constant;
            if ((constant = node.Left as ConstantExpression) != null)
            {
                if (constant.Value is bool)
                {
                    bool value = (bool)constant.Value;

                    if (value && (node.NodeType == ExpressionType.And || node.NodeType == ExpressionType.AndAlso))
                    {
                        BinaryExpression right = node.Right as BinaryExpression;

                        if (right != null)
                        {
                            return this.VisitBinary(right);
                        }

                        return node.Right;
                    }
                }
            }

            if ((constant = node.Right as ConstantExpression) != null)
            {
                if (constant.Value is bool)
                {
                    bool value = (bool)constant.Value;

                    if (value && (node.NodeType == ExpressionType.And || node.NodeType == ExpressionType.AndAlso))
                    {
                        BinaryExpression left = node.Left as BinaryExpression;

                        if (left != null)
                        {
                            return this.VisitBinary(left);
                        }

                        return node.Left;
                    }
                }
            }

            return base.VisitBinary(node);
        }
    }
}
