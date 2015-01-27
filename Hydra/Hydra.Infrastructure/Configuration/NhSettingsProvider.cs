namespace Hydra.Infrastructure.Configuration
{
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using NHibernate;
    using NHibernate.Linq;

    public class NhSettingsProvider : ISettingsProvider
    {
        private readonly ISession session;

        public NhSettingsProvider(ISession session)
        {
            Contract.Requires(session != null);

            this.session = session;
        }

        public SettingsCollection GetSettings()
        {
            var x = this.session.Query<PersistentSetting>().ToArray();

            var settings = x
                .Select(ps => ConvertHelper.Convert(ps, typeof(Setting), CultureInfo.InvariantCulture))
                .OfType<Setting>()
                .ToArray();

            return new SettingsCollection(settings);
        }
    }
}