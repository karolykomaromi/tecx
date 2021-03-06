namespace Hydra.Infrastructure.Configuration
{
    using System.ComponentModel;
    using System.Diagnostics;

    [TypeConverter(typeof(PersistentSettingTypeConverter))]
    [DebuggerDisplay("Id={Id} Name={Name}")]
    public class PersistentSetting : Entity
    {
        public virtual string Name { get; set; }

        public virtual byte[] Value { get; set; }
    }
}