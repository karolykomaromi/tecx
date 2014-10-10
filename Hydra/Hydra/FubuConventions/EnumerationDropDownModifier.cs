namespace Hydra.FubuConventions
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using FubuMVC.Core.UI;
    using FubuMVC.Core.UI.Elements;
    using HtmlTags;
    using Hydra.Infrastructure;
    using Hydra.Infrastructure.Reflection;

    public class EnumerationDropDownModifier : IElementModifier
    {
        public bool Matches(ElementRequest token)
        {
            Type propertyType = token.Accessor.PropertyType;

            bool matches = TypeHelper.GetInheritanceHierarchy(propertyType)
                                     .Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Enumeration<>));

            return matches;
        }

        public void Modify(ElementRequest request)
        {
            Type enumeration = request.Accessor.PropertyType;

            request.CurrentTag.RemoveAttr("type");
            request.CurrentTag.TagName("select");
            request.CurrentTag.Append(new HtmlTag("option"));

            foreach (object value in enumeration.GetTypeInfo().DeclaredFields.Where(f => f.IsPublic && f.IsStatic).Select(f => f.GetValue(null)))
            {
                var optionTag = new HtmlTag("option")
                    .Value(((int)value).ToString(CultureInfo.CurrentCulture))
                    .Text(value.ToString());

                request.CurrentTag.Append(optionTag);
            }
        }
    }
}