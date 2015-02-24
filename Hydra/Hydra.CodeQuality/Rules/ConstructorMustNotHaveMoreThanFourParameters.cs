namespace Hydra.CodeQuality.Rules
{
    using System.Linq;
    using StyleCop;
    using StyleCop.CSharp;

    public class ConstructorMustNotHaveMoreThanFourParameters : ElementVisitor
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
                string ctorSignature =
                    ctor.FullNamespaceName.Replace("Root.", string.Empty) +
                    "(" +
                    string.Join(", ", ctor.Parameters.Select(p => p.Type + " " + p.Name)) +
                    ")";

                this.SourceAnalyzer.AddViolation(
                    ctor,
                    ctor.Location,
                    this.RuleName,
                    ctorSignature,
                    ctor.Parameters.Count);
            }

            return true;
        }
    }
}