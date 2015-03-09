namespace Hydra.CodeQuality
{
    using System.Diagnostics.CodeAnalysis;
    using Hydra.CodeQuality.Rules;
    using StyleCop;
    using StyleCop.CSharp;

    [SourceAnalyzer(typeof(CsParser))]
    public class CodeQualityRules : SourceAnalyzer
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
        public override void AnalyzeDocument(CodeDocument document)
        {
            CsDocument csDocument = document as CsDocument;

            if (csDocument == null)
            {
                return;
            }

            this.WalkDocument(csDocument);

            if (csDocument.RootElement == null ||
                csDocument.RootElement.Generated)
            {
                return;
            }

            this.WalkTokens(csDocument);
        }

        private void WalkTokens(CsDocument document)
        {
            MasterList<CsToken> tokens = document.Tokens;

            if (tokens.Count < 1)
            {
                return;
            }

            ITokenVisitor visitor = TokenVisitor.Null;

            for (Node<CsToken> tokenNode = tokens.First; tokenNode != null; tokenNode = tokenNode.Next)
            {
                if (this.Cancel)
                {
                    break;
                }

                if (!tokenNode.Value.Generated)
                {
                    visitor.Visit(tokenNode, null);
                }
            }
        }

        private void WalkDocument(CsDocument document)
        {
            IElementVisitor elementVisitor = new CompositeElementVisitor(
                new ConstructorMustNotHaveMoreThanFourParameters(this),
                new MethodMustNotHaveMoreThanFourParameters(this),
                new IncorrectRethrow(this),
                new AsyncMethodsMustReturnTask(this),
                new DontDeclareSetOnlyProperties(this));

            IStatementVisitor statementVisitor = StatementVisitor.Null;

            IExpressionVisitor expressionVisitor = new DontUseDefaultOperator(this);

            IQueryClauseVisitor queryClauseVisitor = QueryClauseVisitor.Null;

            document.WalkDocument(elementVisitor.Visit, statementVisitor.Visit, expressionVisitor.Visit, queryClauseVisitor.Visit);
        }
    }
}
