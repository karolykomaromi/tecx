namespace TecX.Playground.QueryAbstractionLayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    public class VisitorCache
    {
        private readonly IDictionary<Type, Func<PDOperator, ExpressionVisitor>> factories;

        public VisitorCache()
        {
            this.factories = new Dictionary<Type, Func<PDOperator, ExpressionVisitor>>();
        }

        public bool TryGetVisitor(Type type, PDOperator pdOperator, out ExpressionVisitor visitor)
        {
            // not a class derived from our mandatory framework baseclass
            if (!typeof(PersistentObject).IsAssignableFrom(type))
            {
                visitor = null;
                return false;
            }

            Func<PDOperator, ExpressionVisitor> factory;
            if (!this.factories.TryGetValue(type, out factory))
            {
                // construct a factory method in the form of Func<PDOperator, ExpressionVisitor> and put it in the lookup
                ParameterExpression p = Expression.Parameter(typeof(PDOperator), "op");

                Type concatFiltersType = typeof(ConcatFrameworkFilters<>).MakeGenericType(type);

                ConstructorInfo ctor = concatFiltersType.GetConstructor(new[] { typeof(PDOperator) });

                NewExpression @new = Expression.New(ctor, p);

                Expression<Func<PDOperator, ExpressionVisitor>> factoryExpression =
                    Expression.Lambda<Func<PDOperator, ExpressionVisitor>>(@new, p);

                // after compiling this will run as fast as a standard delegate in the form of 'pdOperator => new ConcatFrameworkFilter(pdOperator)'
                factory = factoryExpression.Compile();

                this.factories.Add(type, factory);
            }

            visitor = factory(pdOperator);
            return true;
        }
    }
}