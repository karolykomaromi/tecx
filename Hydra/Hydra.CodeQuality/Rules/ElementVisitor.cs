namespace Hydra.CodeQuality.Rules
{
    using StyleCop;
    using StyleCop.CSharp;

    public abstract class ElementVisitor : Visitor, IElementVisitor
    {
        public static readonly IElementVisitor Null = new NullElementVisitor();

        protected ElementVisitor(SourceAnalyzer sourceAnalyzer)
            : base(sourceAnalyzer)
        {
        }

        public abstract bool Visit(CsElement element, CsElement parentelement, object context);
    }
}