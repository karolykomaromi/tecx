namespace Hydra.FubuConventions
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using FubuCore.Reflection;
    using FubuMVC.Core.UI.Elements;
    using HtmlTags;
    using HtmlTags.Conventions;

    public static class FubuAspNetTagExtensions
    {
        public static HtmlTag Input<T>(this HtmlHelper<T> helper, Expression<Func<T, object>> expression)
            where T : class
        {
            var generator = FubuAspNetTagExtensions.GetGenerator<T>();

            return generator.InputFor(expression, model: helper.ViewData.Model);
        }

        public static HtmlTag Label<T>(this HtmlHelper<T> helper, Expression<Func<T, object>> expression)
            where T : class
        {
            var generator = FubuAspNetTagExtensions.GetGenerator<T>();

            return generator.LabelFor(expression, model: helper.ViewData.Model);
        }

        public static HtmlTag Display<T>(this HtmlHelper<T> helper, Expression<Func<T, object>> expression)
            where T : class
        {
            var generator = FubuAspNetTagExtensions.GetGenerator<T>();

            return generator.DisplayFor(expression, model: helper.ViewData.Model);
        }

        public static HtmlTag Validator<T>(this HtmlHelper<T> helper, Expression<Func<T, object>> expression)
            where T : class
        {
            //// MVC code don't ask me I just copied
            string expressionText = ExpressionHelper.GetExpressionText(expression);

            string fullHtmlFieldName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expressionText);

            if (!helper.ViewData.ModelState.ContainsKey(fullHtmlFieldName))
            {
                return new NoTag();
            }

            ModelState modelState = helper.ViewData.ModelState[fullHtmlFieldName];
            ModelErrorCollection modelErrorCollection = modelState == null 
                                                            ? null
                                                            : modelState.Errors;

            ModelError error = modelErrorCollection == null || 
                               modelErrorCollection.Count == 0
                                    ? null
                                    : modelErrorCollection.FirstOrDefault(m => !string.IsNullOrEmpty(m.ErrorMessage)) ?? modelErrorCollection[0];

            if (error == null)
            {
                return new NoTag();
            }

            //// End of MVC code

            ITagGeneratorFactory tagGeneratorFactory = DependencyResolver.Current.GetService<ITagGeneratorFactory>();
            ITagGenerator<ElementRequest> tagGenerator = tagGeneratorFactory.GeneratorFor<ElementRequest>();
            ElementRequest request = new ElementRequest(expression.ToAccessor())
                {
                    Model = helper.ViewData.Model
                };

            HtmlTag tag = tagGenerator.Build(request, "Validator");

            tag.Text(error.ErrorMessage);

            return tag;
        }

        public static HtmlTag InputBlock<T>(this HtmlHelper<T> helper, Expression<Func<T, object>> expression, Action<HtmlTag> inputModifier = null, Action<HtmlTag> validatorModifier = null) 
            where T : class
        {
            inputModifier = inputModifier ?? (_ => { });
            validatorModifier = validatorModifier ?? (_ => { });

            HtmlTag divTag = new HtmlTag("div");
            divTag.AddClass("col-md-10");

            HtmlTag inputTag = helper.Input(expression);
            inputModifier(inputTag);

            HtmlTag validatorTag = helper.Validator(expression);
            validatorModifier(validatorTag);

            divTag.Append(inputTag);
            divTag.Append(validatorTag);

            return divTag;
        }

        public static HtmlTag FormBlock<T>(
            this HtmlHelper<T> helper,
            Expression<Func<T, object>> expression,
            Action<HtmlTag> labelModifier = null,
            Action<HtmlTag> inputBlockModifier = null,
            Action<HtmlTag> inputModifier = null,
            Action<HtmlTag> validatorModifier = null)
            where T : class
        {
            labelModifier = labelModifier ?? (_ => { });
            inputBlockModifier = inputBlockModifier ?? (_ => { });

            HtmlTag divTag = new HtmlTag("div");
            divTag.AddClass("form-group");

            HtmlTag labelTag = helper.Label(expression);
            labelModifier(labelTag);

            HtmlTag inputBlockTag = helper.InputBlock(
                expression,
                inputModifier,
                validatorModifier);

            inputBlockModifier(inputBlockTag);

            divTag.Append(labelTag);
            divTag.Append(inputBlockTag);

            return divTag;
        }

        public static HtmlTag RemoveClasses(this HtmlTag tag)
        {
            foreach (string className in tag.GetClasses())
            {
                tag.RemoveClass(className);
            }

            return tag;
        }

        private static IElementGenerator<T> GetGenerator<T>() where T : class
        {
            IElementGenerator<T> generator = DependencyResolver.Current.GetService<IElementGenerator<T>>();

            return generator;
        }
    }
}