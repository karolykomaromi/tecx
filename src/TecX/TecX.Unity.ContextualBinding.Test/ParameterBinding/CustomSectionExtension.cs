namespace TecX.Unity.ContextualBinding.Test.ParameterBinding
{
    using Microsoft.Practices.Unity.Configuration;

    using TecX.Common;

    public class CustomSectionExtension : SectionExtension
    {
        public override void AddExtensions(SectionExtensionContext context)
        {
            Guard.AssertNotNull(context, "context");

            context.AddElement<DestinationDependentConnectionElement>("destination");
        }
    }
}