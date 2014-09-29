namespace Hydra.FubuConventions
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Drawing;
    using System.Web.Mvc;
    using FubuMVC.Core.UI;
    using FubuMVC.Core.UI.Elements;
    using HtmlTags.Conventions;

    public class OverrideHtmlConventions : DefaultHtmlConventions
    {
        public OverrideHtmlConventions()
        {
            this.Validators.Always.BuildBy<SpanValidatorBuilder>();

            this.Editors.Always.AddClass("form-control");
            this.Labels.Always.AddClass("control-label");
            this.Labels.Always.AddClass("col-md-2");

            // Labels
            this.Labels.ModifyForAttribute<DisplayAttribute>((t, a) => t.Text(a.Name));
            this.Labels.IfPropertyIs<bool>().ModifyWith(er => er.CurrentTag.Text(er.OriginalTag.Text() + "?"));

            // Checkbox
            this.Editors.IfPropertyIs<bool>().Attr("type", "checkbox");

            // Color
            this.Editors.IfPropertyIs<Color>().Attr("type", "color");

            //// Date/Time/DateTime/Local DateTime (NodaTime)
            //// Editors.IfPropertyIs<LocalDate>().Attr("type", "date");
            //// Editors.IfPropertyIs<LocalTime>().Attr("type", "time");
            //// Editors.IfPropertyIs<LocalDateTime>().Attr("type", "datetime-local");
            //// Editors.IfPropertyIs<OffsetDateTime>().Attr("type", "datetime");

            this.Editors.If(er => er.Accessor.Name.Contains("Email")).Attr("type", "email");

            this.Editors.IfPropertyIs<Guid>().Attr("type", "hidden");
            this.Editors.IfPropertyIs<Guid?>().Attr("type", "hidden");
            this.Editors.IfPropertyHasAttribute<HiddenInputAttribute>().Attr("type", "hidden");

            //// Similar for other numeric types (integer/floating point).
            //// Editors.IfPropertyIs<decimal?>().ModifyWith(m => m.CurrentTag.Data("pattern", "9{1,9}.99").Data("placeholder", "0.00"));

            this.Editors.If(er => er.Accessor.Name.IndexOf("Password", StringComparison.OrdinalIgnoreCase) > -1).Attr("type", "password");
            this.Editors.HasAttributeValue<DataTypeAttribute>(attr => attr.DataType == DataType.Password).Attr("type", "password");

            // If we want to enforce a specific pattern, we�d use the appropriate data-pattern attribute.
            this.Editors.If(er => er.Accessor.Name.IndexOf("Phone", StringComparison.OrdinalIgnoreCase) > -1).Attr("type", "tel");
            this.Editors.HasAttributeValue<DataTypeAttribute>(attr => attr.DataType == DataType.PhoneNumber).Attr("type", "tel");

            this.Editors.If(er => er.Accessor.Name.IndexOf("Url", StringComparison.OrdinalIgnoreCase) > -1).Attr("type", "url");
            this.Editors.HasAttributeValue<DataTypeAttribute>(attr => attr.DataType == DataType.Url).Attr("type", "url");

            // DropDowns
            this.Editors.Modifier<EnumDropDownModifier>();
            this.Editors.Modifier<EnitityDropDownModifier>();
        }

        protected ElementCategoryExpression Validators
        {
            get
            {
                BuilderSet<ElementRequest> builderSet = this.Library.For<ElementRequest>().Category("Validator").Defaults;

                return new ElementCategoryExpression(builderSet);
            }
        }
    }
}