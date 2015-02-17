namespace Hydra.CodeQuality.Rules
{
    using StyleCop;
    using StyleCop.CSharp;

    public abstract class CsElementVisitor : Visitor, ICsElementVisitor
    {
        public static readonly ICsElementVisitor Null = new NullCsElementVisitor();

        protected CsElementVisitor(SourceAnalyzer sourceAnalyzer)
            : base(sourceAnalyzer)
        {
        }

        public abstract bool Visit(CsElement element, CsElement parentelement, object context);
    }
}