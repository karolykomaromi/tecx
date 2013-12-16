namespace TecX.Build.Rules
{
    using System.Linq;
    using StyleCop;
    using StyleCop.CSharp;
    using TecX.Common;

    public class IncorrectRethrow : CodeVisitor
    {
        private readonly SourceAnalyzer sourceAnalyzer;

        public IncorrectRethrow(SourceAnalyzer sourceAnalyzer)
        {
            Guard.AssertNotNull(sourceAnalyzer, "sourceAnalyzer");

            this.sourceAnalyzer = sourceAnalyzer;
        }

        public override bool Visit(CsElement element, CsElement parentelement, object context)
        {
            if (element.ElementType != ElementType.Method)
            {
                return true;
            }

            Method method = (Method)element;

            foreach (CatchStatement catchStatement in method.ChildStatements.OfType<CatchStatement>())
            {
                VariableDeclarationExpression catchExpression = (VariableDeclarationExpression)catchStatement.CatchExpression;

                VariableDeclaratorExpression foo = catchExpression.Declarators.FirstOrDefault();

                if (foo != null)
                {
                    string exceptionParameterName = foo.Identifier.ToString();

                    foreach (BlockStatement blockStatement in catchStatement.ChildStatements.OfType<BlockStatement>())
                    {
                        for (int i = 0; i < blockStatement.Tokens.Count() - 2; i++)
                        {
                            var t1 = blockStatement.Tokens.ElementAt(i);
                            var t2 = blockStatement.Tokens.ElementAt(i + 2);

                            if (t1.CsTokenType == CsTokenType.Throw && t2.Text == exceptionParameterName)
                            {
                                this.sourceAnalyzer.AddViolation(
                                    method,
                                    "IncorrectRethrow",
                                    method.FullyQualifiedName.Replace("Root.", string.Empty));
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}