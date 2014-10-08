namespace TecX.Query.Visitors
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using TecX.Common;
    using TecX.Query.PD;

    public class VisitorCache
    {
        private readonly Func<Type, Func<PDIteratorOperator, IClientInfo, ExpressionVisitor>> onMissing;

        private readonly IDictionary<Type, Func<PDIteratorOperator, IClientInfo, ExpressionVisitor>> factories;

        public VisitorCache(Func<Type, Func<PDIteratorOperator, IClientInfo, ExpressionVisitor>> onMissing)
        {
            Guard.AssertNotNull(onMissing, "onMissing");

            this.onMissing = onMissing;
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
                factory = this.onMissing(type);

                this.factories.Add(type, factory);
            }

            visitor = factory(pdOperator, clientInfo);
            return true;
        }
    }
}