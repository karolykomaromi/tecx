namespace Hydra.CodeQuality
{
    using System.Diagnostics.CodeAnalysis;
    using Hydra.CodeQuality.Rules;
    using StyleCop;
    using StyleCop.CSharp;

    [SourceAnalyzer(typeof(CsParser))]
    public class SpacingRules : SourceAnalyzer
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
        public override void AnalyzeDocument(CodeDocument document)
        {
            CsDocument csDocument = document as CsDocument;

            if (csDocument == null ||
                csDocument.RootElement == null ||
                csDocument.RootElement.Generated)
            {
                return;
            }

            MasterList<CsToken> tokens = csDocument.Tokens;

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
    }
}
