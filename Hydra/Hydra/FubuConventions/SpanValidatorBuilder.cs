namespace Hydra.FubuConventions
{
    using FubuMVC.Core.UI.Elements;
    using HtmlTags;

    public class SpanValidatorBuilder : IElementBuilder
    {
        public HtmlTag Build(ElementRequest request)
        {
            HtmlTag tag = new HtmlTag("span")
                .AddClass("field-validation-error")
                .AddClass("text-danger")
                .Data("valmsg-for", request.ElementId);

            return tag;
        }
    }
}