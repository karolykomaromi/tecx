using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

using TecX.Playground.QueryAbstractionLayer.PD;

namespace TecX.Playground.QueryAbstractionLayer.Visitors
{
    public class VisitorCache
    {
        private readonly IDictionary<Type, Func<PDIteratorOperator, ExpressionVisitor>> factories;

        public VisitorCache()
        {
            this.factories = new Dictionary<Type, Func<PDIteratorOperator, ExpressionVisitor>>();
        }

        public bool TryGetVisitor(Type type, PDIteratorOperator pdOperator, out ExpressionVisitor visitor)
        {
            // not a class derived from our mandatory framework baseclass
            if (!typeof(PersistentObject).IsAssignableFrom(type))
            {
                visitor = null;
                return false;
            }

            Func<PDIteratorOperator, ExpressionVisitor> factory;
            if (!this.factories.TryGetValue(type, out factory))
            {
                // construct a factory method in the form of Func<PDOperator, ExpressionVisitor> and put it in the lookup
                ParameterExpression p = Expression.Parameter(typeof(PDIteratorOperator), "op");

                Type concatFiltersType = typeof(AppendFrameworkFilters<>).MakeGenericType(type);

                ConstructorInfo ctor = concatFiltersType.GetConstructor(new[] { typeof(PDIteratorOperator) });

                NewExpression @new = Expression.New(ctor, p);

                Expression<Func<PDIteratorOperator, ExpressionVisitor>> factoryExpression =
                    Expression.Lambda<Func<PDIteratorOperator, ExpressionVisitor>>(@new, p);

                // after compiling this will run as fast as a standard delegate in the form of 'pdOperator => new ConcatFrameworkFilter(pdOperator)'
                factory = factoryExpression.Compile();

                this.factories.Add(type, factory);
            }

            visitor = factory(pdOperator);
            return true;
        }
    }
}