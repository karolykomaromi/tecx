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

            ICsElementVisitor visitor = new CompositeCsElementVisitor(
                new ConstructorMustNotHaveMoreThanFourParameters(this),
                new MethodMustNotHaveMoreThanFourParameters(this),
                new IncorrectRethrow(this));

            csDocument.WalkDocument(visitor.Visit);
        }
    }
}
