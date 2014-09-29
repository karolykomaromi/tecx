namespace Hydra
{
    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using FubuMVC.Core.UI.Elements;
    using HtmlTags;

    public static class FubuAspNetTagExtensions
    {
        // Similar methods for Display/Label
        public static HtmlTag Input<T>(this HtmlHelper<T> helper, Expression<Func<T, object>> expression) where T : class
        {
            var generator = GetGenerator<T>();

            return generator.InputFor(expression, model: helper.ViewData.Model);
        }

        public static HtmlTag Label<T>(this HtmlHelper<T> helper, Expression<Func<T, object>> expression) where T : class
        {
            var generator = GetGenerator<T>();

            return generator.LabelFor(expression, model: helper.ViewData.Model);
        }

        public static HtmlTag Display<T>(this HtmlHelper<T> helper, Expression<Func<T, object>> expression) where T : class
        {
            var generator = GetGenerator<T>();

            return generator.DisplayFor(expression, model: helper.ViewData.Model);
        }

        private static IElementGenerator<T> GetGenerator<T>() where T : class
        {
            IElementGenerator<T> generator = DependencyResolver.Current.GetService<IElementGenerator<T>>();

            return generator;
        }
    }
}