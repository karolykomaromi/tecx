namespace Hydra.Infrastructure.I18n
{
    using FluentNHibernate.Mapping;

    public class ResourceItemMap : ClassMap<ResourceItem>
    {
        public ResourceItemMap()
        {
            this.Id(x => x.Id);
            this.Map(x => x.Name).Index("name_culture");
            this.Map(x => x.TwoLetterISOLanguageName).Index("name_culture");
            this.Map(x => x.Value);
        }
    }
}