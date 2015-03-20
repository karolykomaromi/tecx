namespace Hydra.Infrastructure.Configuration
{
    public class PersistentSettingMap : EntityMap<PersistentSetting>
    {
        public PersistentSettingMap()
        {
            this.Map(x => x.Name);
            this.Map(x => x.Value);
        }
    }
}