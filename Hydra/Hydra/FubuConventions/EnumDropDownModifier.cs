using Hydra.Infrastructure;

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

            // TODO weberse 2014-09-29 Check wether it derives from Enumeration<T>
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
                var optionTag = new HtmlTag("option")
                    .Value(value.ToString())
                    .Text(Enum.GetName(enumType, value));

                request.CurrentTag.Append(optionTag);
            }
        }
    }

    ////public class EnumerationDropDownModifier : IElementModifier
    ////{
    ////    public bool Matches(ElementRequest token)
    ////    {
    ////        Type propertyType = token.Accessor.PropertyType;

    ////        try
    ////        {
    ////            Type enumerationClassType = typeof(Enumeration<>).MakeGenericType(propertyType);

    ////            return enumerationClassType.IsAssignableFrom(propertyType);
    ////        }
    ////        catch (ArgumentException)
    ////        {
    ////        }

    ////        return false;
    ////    }

    ////    public void Modify(ElementRequest request)
    ////    {
    ////        var enumType = request.Accessor.PropertyType;

    ////        request.CurrentTag.RemoveAttr("type");
    ////        request.CurrentTag.TagName("select");
    ////        request.CurrentTag.Append(new HtmlTag("option"));

    ////        foreach (var value in Enum.GetValues(enumType))
    ////        {
    ////            var optionTag = new HtmlTag("option")
    ////                .Value(value.ToString())
    ////                .Text(Enum.GetName(enumType, value));

    ////            request.CurrentTag.Append(optionTag);
    ////        }
    ////    }
    ////}
}