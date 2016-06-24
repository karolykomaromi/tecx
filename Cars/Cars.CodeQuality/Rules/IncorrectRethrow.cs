namespace Cars.CodeQuality.Rules
{
    using System.Linq;
    using StyleCop;
    using StyleCop.CSharp;

    public class IncorrectRethrow : ElementVisitor
    {
        public IncorrectRethrow(SourceAnalyzer sourceAnalyzer)
            : base(sourceAnalyzer)
        {
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
                if (catchStatement.CatchExpression == null)
                {
                    // weberse 2015-08-21 catch-statement without exception type (catch { })
                    continue;
                }

                if (catchStatement.CatchExpression.ExpressionType == ExpressionType.Literal)
                {
                    // weberse 2015-08-21 catch-statement with exception type but w/o variable declaration (catch(FileNotFoundException) { })
                    continue;
                }

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
                                this.SourceAnalyzer.AddViolation(
                                    method,
                                    method.Location,
                                    this.RuleName,
                                    method.FullyQualifiedName.Replace("Root.", string.Empty),
                                    exceptionParameterName);
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}