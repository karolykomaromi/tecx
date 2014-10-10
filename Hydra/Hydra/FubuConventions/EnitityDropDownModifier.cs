namespace Hydra.FubuConventions
{
    using System.Diagnostics.Contracts;
    using FubuMVC.Core.UI.Elements;
    using Hydra.Queries;

    public class EnitityDropDownModifier : IElementModifier
    {
        private readonly IMediator mediator;

        public EnitityDropDownModifier(IMediator mediator)
        {
            Contract.Requires(mediator != null);

            this.mediator = mediator;
        }

        public bool Matches(ElementRequest token)
        {
            // TODO weberse 2014-10-10 check wether there is a query matching the (pluralized?) property name (with parameterless ctor?). cache result.

            ////return typeof(Entity).IsAssignableFrom(token.Accessor.PropertyType);
            return false;
            ////return true;
        }

        public void Modify(ElementRequest request)
        {
            // TODO weberse 2014-10-10 use the mediator to create 

            ////request.CurrentTag.RemoveAttr("type");
            ////request.CurrentTag.TagName("select");
            ////request.CurrentTag.Append(new HtmlTag("option"));

            ////var context = request.Get<DbContext>();
            ////var entities = context.Set(request.Accessor.PropertyType)
            ////.Cast<Entity>()
            ////.ToList();
            ////var value = request.Value<Entity>();

            ////foreach (var entity in entities)
            ////{
            ////    var optionTag = new HtmlTag("option")
            ////    .Value(entity.Id.ToString())
            ////    .Text(entity.DisplayValue);

            ////    if (value != null && value.Id == entity.Id)
            ////        optionTag.Attr("selected");

            ////    request.CurrentTag.Append(optionTag);
            ////}
        }
    }
}