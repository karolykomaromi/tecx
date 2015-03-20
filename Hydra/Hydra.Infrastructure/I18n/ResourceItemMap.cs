namespace Hydra.Infrastructure.I18n
{
    using FluentNHibernate.Mapping;

    public class ResourceItemMap : EntityMap<ResourceItem>
    {
        public ResourceItemMap()
        {
            this.Map(x => x.Name).Index("name_culture");
            this.Map(x => x.Language).Index("name_culture");
            this.Map(x => x.Value);
        }
    }
}