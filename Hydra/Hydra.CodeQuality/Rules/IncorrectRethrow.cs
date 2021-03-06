namespace Hydra.CodeQuality.Rules
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
            Method method = element as Method;

            if (method == null)
            {
                return true;
            }

            foreach (CatchStatement catchStatement in method.ChildStatements.OfType<CatchStatement>())
            {
                VariableDeclarationExpression catchExpression = catchStatement.CatchExpression as VariableDeclarationExpression;

                if (catchExpression == null)
                {
                    return true;
                }

                VariableDeclaratorExpression @catch = catchExpression.Declarators.FirstOrDefault();

                if (@catch != null)
                {
                    string exceptionParameterName = @catch.Identifier.ToString();

                    foreach (BlockStatement blockStatement in catchStatement.ChildStatements.OfType<BlockStatement>())
                    {
                        for (int i = 0; i < blockStatement.Tokens.Count() - 2; i++)
                        {
                            var t1 = blockStatement.Tokens.ElementAt(i);
                            var t2 = blockStatement.Tokens.ElementAt(i + 2);

                            if (t1.CsTokenType == CsTokenType.Throw && t2.Text == exceptionParameterName)
                            {
                                string methodSignature =
                                    method.FullNamespaceName.Replace("Root.", string.Empty) +
                                    "(" +
                                    string.Join(", ", method.Parameters.Select(p => p.Type + " " + p.Name)) +
                                    ")";

                                this.SourceAnalyzer.AddViolation(
                                    method,
                                    method.Location,
                                    this.RuleName,
                                    methodSignature,
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