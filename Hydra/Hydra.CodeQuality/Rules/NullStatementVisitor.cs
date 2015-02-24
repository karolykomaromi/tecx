namespace Hydra.CodeQuality.Rules
{
    using StyleCop.CSharp;

    public class NullStatementVisitor : IStatementVisitor
    {
        public bool Visit(
            Statement statement, 
            Expression parentExpression, 
            Statement parentStatement, 
            CsElement parentElement, 
            object context)
        {
            return true;
        }
    }
}