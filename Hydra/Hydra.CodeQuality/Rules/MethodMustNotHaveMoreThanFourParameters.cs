namespace Hydra.CodeQuality.Rules
{
    using StyleCop;
    using StyleCop.CSharp;

    public class MethodMustNotHaveMoreThanFourParameters : ElementVisitor
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
                string methodSignature = MethodHelper.Signature(method);

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