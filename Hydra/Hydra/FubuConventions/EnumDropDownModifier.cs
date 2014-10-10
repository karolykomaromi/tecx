namespace Hydra.FubuConventions
{
    using System;
    using FubuMVC.Core.UI;
    using FubuMVC.Core.UI.Elements;
    using HtmlTags;

    public class EnumDropDownModifier : IElementModifier
    {
        public bool Matches(ElementRequest token)
        {
            Type propertyType = token.Accessor.PropertyType;

            return propertyType.IsEnum;
        }

        public void Modify(ElementRequest request)
        {
            var enumType = request.Accessor.PropertyType;

            request.CurrentTag.RemoveAttr("type");
            request.CurrentTag.TagName("select");
            request.CurrentTag.Append(new HtmlTag("option"));

            foreach (var value in Enum.GetValues(enumType))
            {
                // TODO weberse 2014-10-10 try to find a translation in the resources. which resources should be used? enum might be 3rd party
                var optionTag = new HtmlTag("option")
                    .Value(value.ToString())
                    .Text(Enum.GetName(enumType, value));

                request.CurrentTag.Append(optionTag);
            }
        }
    }
}