namespace Hydra.Features.Settings
{
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using Hydra.Infrastructure.Configuration;
    using Hydra.Queries;

    public class SettingsQueryHandler : IQueryHandler<AllSettingsQuery, SettingsCollection>, IQueryHandler<SettingByNameQuery, Setting>
    {
        private readonly ISettingsProvider settingsProvider;

        public SettingsQueryHandler(ISettingsProvider settingsProvider)
        {
            Contract.Requires(settingsProvider != null);

            this.settingsProvider = settingsProvider;
        }

        public async Task<SettingsCollection> Handle(AllSettingsQuery query)
        {
            Contract.Requires(query != null);

            return await Task<SettingsCollection>.Factory.StartNew(() => this.settingsProvider.GetSettings());
        }

        public async Task<Setting> Handle(SettingByNameQuery query)
        {
            Contract.Requires(query != null);
            Contract.Requires(query.Name != null);

            return await Task<Setting>.Factory.StartNew(() => this.settingsProvider.GetSettings()[query.Name]);
        }
    }
}