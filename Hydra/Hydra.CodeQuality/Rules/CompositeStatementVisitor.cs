namespace Hydra.CodeQuality.Rules
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using StyleCop.CSharp;

    public class CompositeStatementVisitor : IStatementVisitor
    {
        private readonly List<IStatementVisitor> visitors;

        public CompositeStatementVisitor(params IStatementVisitor[] visitors)
        {
            this.visitors = new List<IStatementVisitor>(visitors ?? new IStatementVisitor[0]);
        }

        [SuppressMessage("Hydra.CodeQuality.CodeQualityRules", "HD1001:MethodMustNotHaveMoreThanFourParameters", Justification = "Needed to work with StyleCop CodeWalkerStatementVisitor<T>")]
        public bool Visit(
            Statement statement,
            Expression parentExpression,
            Statement parentStatement,
            CsElement parentElement,
            object context)
        {
            bool b = true;

            this.visitors.ForEach(visitor => b &= visitor.Visit(
                statement, 
                parentExpression, 
                parentStatement, 
                parentElement, 
                context));

            return b;
        }
    }
}