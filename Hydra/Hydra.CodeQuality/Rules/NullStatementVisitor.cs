namespace Hydra.CodeQuality.Rules
{
    using System.Diagnostics.CodeAnalysis;
    using StyleCop.CSharp;

    public class NullStatementVisitor : IStatementVisitor
    {
        [SuppressMessage("Hydra.CodeQuality.CodeQualityRules", "HD1001:MethodMustNotHaveMoreThanFourParameters", Justification = "Needed to work with StyleCop CodeWalkerStatementVisitor<T>")]
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