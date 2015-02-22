namespace Hydra.FubuConventions
{
    using System;
    using System.ComponentModel;
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

            TypeConverter converter = TypeDescriptor.GetConverter(enumeration);

            foreach (object value in enumeration.GetTypeInfo().DeclaredFields.Where(f => f.IsPublic && f.IsStatic).Select(f => f.GetValue(null)))
            {
                object converted = converter.ConvertTo(value, typeof(int));

                int v = converted is int ? (int)converted : Default.Value<int>();

                var optionTag = new HtmlTag("option")
                    .Value(v.ToString(CultureInfo.CurrentCulture))
                    .Text(value.ToString());

                request.CurrentTag.Append(optionTag);
            }
        }
    }
}