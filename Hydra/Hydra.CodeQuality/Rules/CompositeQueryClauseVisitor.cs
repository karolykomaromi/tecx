namespace Hydra.CodeQuality.Rules
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using StyleCop.CSharp;

    public class CompositeQueryClauseVisitor : IQueryClauseVisitor
    {
        private readonly List<IQueryClauseVisitor> visitors;

        public CompositeQueryClauseVisitor(params IQueryClauseVisitor[] visitors)
        {
            this.visitors = new List<IQueryClauseVisitor>(visitors ?? new IQueryClauseVisitor[0]);
        }

        [SuppressMessage("Hydra.CodeQuality.CodeQualityRules", "HD1001:MethodMustNotHaveMoreThanFourParameters", Justification = "Needed to work with StyleCop CodeWalkerQueryClauseVisitor<T>")]
        public bool Visit(
            QueryClause clause, 
            QueryClause parentClause, 
            Expression parentExpression, 
            Statement parentStatement,
            CsElement parentElement, 
            object context)
        {
            bool b = true;

            this.visitors.ForEach(visitor => b &= visitor.Visit(
                clause, 
                parentClause, 
                parentExpression, 
                parentStatement, 
                parentElement, 
                context));

            return b;
        }
    }
}