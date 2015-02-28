namespace Hydra.CodeQuality.Rules
{
    using System.Diagnostics.CodeAnalysis;
    using StyleCop;
    using StyleCop.CSharp;

    public abstract class StatementVisitor : Visitor, IStatementVisitor
    {
        public static readonly IStatementVisitor Null = new NullStatementVisitor();

        protected StatementVisitor(SourceAnalyzer sourceAnalyzer)
            : base(sourceAnalyzer)
        {
        }

        [SuppressMessage("Hydra.CodeQuality.CodeQualityRules", "HD1001:MethodMustNotHaveMoreThanFourParameters", Justification = "Needed to work with StyleCop CodeWalkerStatementVisitor<T>")]
        public abstract bool Visit(
            Statement statement, 
            Expression parentExpression, 
            Statement parentStatement, 
            CsElement parentElement, 
            object context);
    }
}