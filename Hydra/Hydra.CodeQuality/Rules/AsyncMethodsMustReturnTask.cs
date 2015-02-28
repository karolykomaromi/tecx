namespace Hydra.CodeQuality.Rules
{
    using StyleCop;
    using StyleCop.CSharp;

    public class AsyncMethodsMustReturnTask : ElementVisitor
    {
        public AsyncMethodsMustReturnTask(SourceAnalyzer sourceAnalyzer)
            : base(sourceAnalyzer)
        {
        }

        public override bool Visit(CsElement element, CsElement parentelement, object context)
        {
            Method method = element as Method;

            if (method != null && IsAsyncVoidMethod(method))
            {
                string methodSignature = MethodHelper.Signature(method);

                this.SourceAnalyzer.AddViolation(
                    method,
                    method.Location,
                    this.RuleName,
                    methodSignature);
            }

            return true;
        }

        private static bool IsAsyncVoidMethod(Method method)
        {
            return method.ReturnType != null &&
                   method.ReturnType.Text == "void" && 
                   method.Declaration != null &&
                   method.Declaration.ContainsModifier(new[] { CsTokenType.Async });
        }
    }
}