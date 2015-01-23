namespace Hydra.Infrastructure.Configuration
{
    using System.Diagnostics.Contracts;
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
            var settings = this.session.Query<Setting>().ToArray();

            return new SettingsCollection(settings);
        }
    }
}