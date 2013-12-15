namespace TecX.Query.Visitors
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using TecX.Query.PD;

    public class ElementTypeFinder : ExpressionVisitor
    {
        public Type ElementType { get; private set; }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.IsGenericMethod)
            {
                Type elementType = node.Method.GetGenericArguments().FirstOrDefault(type => typeof(PersistentObject).IsAssignableFrom(type));

                if (elementType != null && this.ElementType == null)
                {
                    this.ElementType = elementType;
                }
            }

            return base.VisitMethodCall(node);
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            Type type = typeof(T);

            if (type.IsGenericType && typeof(Func<,>).IsAssignableFrom(type.GetGenericTypeDefinition()))
            {
                Type[] genericArguments = type.GetGenericArguments();

                if (typeof(PersistentObject).IsAssignableFrom(genericArguments[0]) &&
                    typeof(bool).IsAssignableFrom(genericArguments[1]))
                {
                    if (this.ElementType == null)
                    {
                        this.ElementType = genericArguments[0];
                    }
                    else
                    {
                        // TODO weberse 2013-07-08 found a second lambda?!
                    }
                }
            }

            return base.VisitLambda<T>(node);
        }
    }
}