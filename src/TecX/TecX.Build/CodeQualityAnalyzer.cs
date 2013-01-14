namespace TecX.Build
{
	using StyleCop;
	using StyleCop.CSharp;

	using TecX.Build.Rules;

	[SourceAnalyzer(typeof(CsParser))]
	public class CodeQualityAnalyzer : SourceAnalyzer
	{
		public override void AnalyzeDocument(CodeDocument document)
		{
			CsDocument csDocument = document as CsDocument;

			if (csDocument == null)
			{
				return;
			}

			var visitor = new CompositeVisitor(new CodeVisitor[]
				{
					new ConstructorMustNotHaveMoreThanFourParameters(this)
				});

			csDocument.WalkDocument(new CodeWalkerElementVisitor<object>(visitor.Visit));
		}
	}
}
