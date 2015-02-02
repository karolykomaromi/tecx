namespace Hydra.FubuConventions
{
    using System;
    using FubuCore.Reflection;
    using FubuMVC.Core.UI;

    public static class ElementCategoryExpressionExtensions
    {
        public static ElementActionExpression HasAttributeValue<TAttribute>(this ElementCategoryExpression expression, Func<TAttribute, bool> matches) where TAttribute : Attribute
        {
            return expression.If(er =>
                {
                    TAttribute attr = er.Accessor.GetAttribute<TAttribute>();

                    return attr != null && matches(attr);
                });
        }
    }
}