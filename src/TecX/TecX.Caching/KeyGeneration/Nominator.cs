namespace TecX.Caching.KeyGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Performs bottom-up analysis to determine which nodes can possibly
    /// be part of an evaluated sub-tree.
    /// </summary>    
    /// <remarks>
    /// Copyright (c) 2010 Pete Montgomery.
    /// http://petemontgomery.wordpress.com
    /// Licenced under GNU LGPL v3.
    /// http://www.gnu.org/licenses/lgpl.html
    /// </remarks>
    internal class Nominator : ExpressionVisitor
    {
        private readonly Func<Expression, bool> canBeEvaluated;

        private HashSet<Expression> candidates;

        private bool cannotBeEvaluated;

        internal Nominator(Func<Expression, bool> canBeEvaluated)
        {
            this.canBeEvaluated = canBeEvaluated;
        }

        public override Expression Visit(Expression expression)
        {
            if (expression != null)
            {
                bool saveCannotBeEvaluated = this.cannotBeEvaluated;
                this.cannotBeEvaluated = false;
                base.Visit(expression);
                if (!this.cannotBeEvaluated)
                {
                    if (this.canBeEvaluated(expression))
                    {
                        this.candidates.Add(expression);
                    }
                    else
                    {
                        this.cannotBeEvaluated = true;
                    }
                }

                this.cannotBeEvaluated |= saveCannotBeEvaluated;
            }

            return expression;
        }

        internal HashSet<Expression> Nominate(Expression expression)
        {
            this.candidates = new HashSet<Expression>();
            this.Visit(expression);
            return this.candidates;
        }
    }
}