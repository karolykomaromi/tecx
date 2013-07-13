namespace TecX.Query.Strategies
{
    using System;
    using System.Linq.Expressions;

    public abstract class ExpressionManipulationStrategy
    {
        public abstract Expression Process(Expression expression, Type elementType);
    }
}