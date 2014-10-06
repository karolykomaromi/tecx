namespace TecX.Caching.KeyGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Evaluates & replaces sub-trees when first candidate is reached (top-down)
    /// </summary>    
    /// <remarks>
    /// Copyright (c) 2010 Pete Montgomery.
    /// http://petemontgomery.wordpress.com
    /// Licenced under GNU LGPL v3.
    /// http://www.gnu.org/licenses/lgpl.html
    /// </remarks>
    internal class SubtreeEvaluator : ExpressionVisitor
    {
        private readonly HashSet<Expression> candidates;

        internal SubtreeEvaluator(HashSet<Expression> candidates)
        {
            this.candidates = candidates;
        }

        public override Expression Visit(Expression exp)
        {
            if (exp == null)
            {
                return null;
            }

            if (this.candidates.Contains(exp))
            {
                return this.Evaluate(exp);
            }

            return base.Visit(exp);
        }

        internal Expression Eval(Expression exp)
        {
            return this.Visit(exp);
        }

        private Expression Evaluate(Expression e)
        {
            if (e.NodeType == ExpressionType.Constant)
            {
                return e;
            }

            LambdaExpression lambda = Expression.Lambda(e);
            Delegate fn = lambda.Compile();
            return Expression.Constant(fn.DynamicInvoke(null), e.Type);
        }
    }
}