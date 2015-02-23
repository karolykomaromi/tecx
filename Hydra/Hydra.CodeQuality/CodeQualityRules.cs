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

            this.VisitElements(csDocument);

            if (csDocument.RootElement == null ||
                csDocument.RootElement.Generated)
            {
                return;
            }

            this.VisitTokens(csDocument);
        }

        private void VisitTokens(CsDocument document)
        {
            MasterList<CsToken> tokens = document.Tokens;

            if (tokens.Count < 1)
            {
                return;
            }

            ICsTokenVisitor visitor = new NullCsTokenVisitor();

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

        private void VisitElements(CsDocument document)
        {
            ICsElementVisitor elementVisitor = new CompositeCsElementVisitor(
                new ConstructorMustNotHaveMoreThanFourParameters(this),
                new MethodMustNotHaveMoreThanFourParameters(this),
                new IncorrectRethrow(this));

            IExpressionVisitor expressionVisitor = new DontUseDefaultOperator(this);

            document.WalkDocument(elementVisitor.Visit, (CodeWalkerStatementVisitor<object>)null, expressionVisitor.Visit);
        }
    }
}
