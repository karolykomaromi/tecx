namespace TecX.Playground.QueryAbstractionLayer.Visitors
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    using TecX.Common;
    using TecX.Playground.QueryAbstractionLayer.PD;

    public class VisitorCache
    {
        private readonly IDictionary<Type, Func<PDIteratorOperator, IClientInfo, ExpressionVisitor>> factories;

        public VisitorCache()
        {
            this.factories = new Dictionary<Type, Func<PDIteratorOperator, IClientInfo, ExpressionVisitor>>();
        }

        public bool TryGetVisitor(Type type, PDIteratorOperator pdOperator, IClientInfo clientInfo, out ExpressionVisitor visitor)
        {
            Guard.AssertNotNull(pdOperator, "pdOperator");
            Guard.AssertNotNull(clientInfo, "clientInfo");

            // not a class derived from our mandatory framework baseclass
            if (type == null ||
                !typeof(PersistentObject).IsAssignableFrom(type))
            {
                visitor = null;
                return false;
            }

            Func<PDIteratorOperator, IClientInfo, ExpressionVisitor> factory;
            if (!this.factories.TryGetValue(type, out factory))
            {
                // construct a factory method in the form of Func<PDOperator, ExpressionVisitor> and put it in the lookup
                ParameterExpression p1 = Expression.Parameter(typeof(PDIteratorOperator), "op");

                ParameterExpression p2 = Expression.Parameter(typeof(IClientInfo), "clientInfo");

                Type concatFiltersType = typeof(AppendFrameworkFilters<>).MakeGenericType(type);

                ConstructorInfo ctor = concatFiltersType.GetConstructor(new[] { typeof(PDIteratorOperator), typeof(IClientInfo) });

                NewExpression @new = Expression.New(ctor, p1, p2);

                Expression<Func<PDIteratorOperator, IClientInfo, ExpressionVisitor>> factoryExpression =
                    Expression.Lambda<Func<PDIteratorOperator, IClientInfo, ExpressionVisitor>>(@new, p1, p2);

                // after compiling this will run as fast as a standard delegate in the form of 'pdOperator => new ConcatFrameworkFilter(pdOperator)'
                factory = factoryExpression.Compile();

                this.factories.Add(type, factory);
            }

            visitor = factory(pdOperator, clientInfo);
            return true;
        }
    }
}