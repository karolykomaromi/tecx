namespace Hydra.CodeQuality.Rules
{
    using StyleCop;
    using StyleCop.CSharp;

    public abstract class StatementVisitor : Visitor, IStatementVisitor
    {
        public static readonly IStatementVisitor Null = new NullStatementVisitor();

        protected StatementVisitor(SourceAnalyzer sourceAnalyzer)
            : base(sourceAnalyzer)
        {
        }

        public abstract bool Visit(
            Statement statement, 
            Expression parentExpression, 
            Statement parentStatement, 
            CsElement parentElement, 
            object context);
    }
}