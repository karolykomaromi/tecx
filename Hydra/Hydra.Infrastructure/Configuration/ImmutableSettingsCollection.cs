namespace Hydra.Infrastructure.Configuration
{
    using System.Diagnostics.Contracts;

    public class ImmutableSettingsCollection : SettingsCollection
    {
        public ImmutableSettingsCollection(SettingsCollection settings)
        {
            Contract.Requires(settings != null);

            foreach (Setting setting in settings)
            {
                base.Add(setting);
            }
        }

        public override bool IsMutable
        {
            get { return false; }
        }

        public override SettingsCollection Freeze()
        {
            return this;
        }

        public override void Add(Setting setting)
        {
        }

        public override SettingsCollection Merge(SettingsCollection winner)
        {
            return this;
        }
    }
}