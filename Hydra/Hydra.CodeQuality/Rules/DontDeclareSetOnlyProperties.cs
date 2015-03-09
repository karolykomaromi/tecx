namespace Hydra.CodeQuality.Rules
{
    using StyleCop;
    using StyleCop.CSharp;

    public class DontDeclareSetOnlyProperties : ElementVisitor
    {
        public DontDeclareSetOnlyProperties(SourceAnalyzer sourceAnalyzer)
            : base(sourceAnalyzer)
        {
        }

        public override bool Visit(CsElement element, CsElement parentelement, object context)
        {
            Property property = element as Property;

            if (IsSetOnlyProperty(property))
            {
                this.SourceAnalyzer.AddViolation(
                    property,
                    property.Location,
                    this.RuleName,
                    (System.Enum.GetName(typeof(AccessModifierType), property.AccessModifier) ?? string.Empty).ToLowerInvariant(),
                    property.Name.Replace(property.FriendlyTypeText + " ", string.Empty),
                    property.ReturnType);
            }

            return true;
        }

        private static bool IsSetOnlyProperty(Property property)
        {
            return property != null &&
                   property.GetAccessor == null &&
                   property.SetAccessor != null;
        }
    }
}