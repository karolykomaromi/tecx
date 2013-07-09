namespace TecX.Playground.QueryAbstractionLayer.Visitors
{
    using System;
    using System.Linq.Expressions;

    using TecX.Playground.QueryAbstractionLayer.PD;

    public class ElementTypeFinder : ExpressionVisitor
    {
        public Type ElementType { get; private set; }

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