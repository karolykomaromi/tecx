namespace TecX.Build.Rules
{
    using Common;
    using StyleCop;
    using StyleCop.CSharp;

    public class ConstructorMustNotHaveMoreThanFourParameters : CodeVisitor
    {
        private readonly SourceAnalyzer sourceAnalyzer;

        public ConstructorMustNotHaveMoreThanFourParameters(SourceAnalyzer sourceAnalyzer)
        {
            Guard.AssertNotNull(sourceAnalyzer, "sourceAnalyzer");

            this.sourceAnalyzer = sourceAnalyzer;
        }

        public override bool Visit(CsElement element, CsElement parentelement, object context)
        {
            if (element.ElementType != ElementType.Constructor)
            {
                return true;
            }

            Constructor ctor = (Constructor)element;

            if (ctor.Parameters.Count > 4)
            {
                this.sourceAnalyzer.AddViolation(
                    ctor,
                    "TooManyConstructorArguments",
                    ctor.FindParentElement().FullyQualifiedName.Replace("Root.", string.Empty),
                    ctor.Parameters.Count);
            }

            return true;
        }
    }
}