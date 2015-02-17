namespace Hydra.CodeQuality.Rules
{
    using System.Linq;
    using StyleCop;
    using StyleCop.CSharp;

    public class MethodMustNotHaveMoreThanFourParameters : CsElementVisitor
    {
        public MethodMustNotHaveMoreThanFourParameters(SourceAnalyzer sourceAnalyzer)
            : base(sourceAnalyzer)
        {
        }

        public override bool Visit(CsElement element, CsElement parentelement, object context)
        {
            Method method = element as Method;

            if (method != null &&
                method.Parameters.Count > 4)
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
                    method.Parameters.Count);
            }

            return true;
        }
    }
}