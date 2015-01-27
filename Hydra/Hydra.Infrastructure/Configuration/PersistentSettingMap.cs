namespace Hydra.Infrastructure.Configuration
{
    using FluentNHibernate.Mapping;

    public class PersistentSettingMap : ClassMap<PersistentSetting>
    {
        public PersistentSettingMap()
        {
            this.Id(x => x.Id);
            this.Map(x => x.Name);
            this.Map(x => x.Value);
        }
    }
}