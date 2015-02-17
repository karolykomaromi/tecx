namespace Hydra.CodeQuality.Rules
{
    using StyleCop;
    using StyleCop.CSharp;

    public class ConstructorMustNotHaveMoreThanFourParameters : CsElementVisitor
    {
        public ConstructorMustNotHaveMoreThanFourParameters(SourceAnalyzer sourceAnalyzer)
            : base(sourceAnalyzer)
        {
        }

        public override bool Visit(CsElement element, CsElement parentelement, object context)
        {
            Constructor ctor = element as Constructor;

            if (ctor != null &&
                ctor.Parameters.Count > 4)
            {
                this.SourceAnalyzer.AddViolation(
                    ctor,
                    ctor.Location,
                    this.RuleName,
                    ctor.FindParentElement().FullyQualifiedName.Replace("Root.", string.Empty),
                    ctor.Parameters.Count);
            }

            return true;
        }
    }
}