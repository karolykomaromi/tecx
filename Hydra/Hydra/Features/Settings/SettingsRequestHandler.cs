namespace Hydra.Features.Settings
{
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using Hydra.Infrastructure.Configuration;
    using Hydra.Infrastructure.Mediator;

    public class SettingsRequestHandler : IRequestHandler<AllSettingsQuery, SettingsCollection>, IRequestHandler<SettingByNameRequest, Setting>
    {
        private readonly ISettingsProvider settingsProvider;

        public SettingsRequestHandler(ISettingsProvider settingsProvider)
        {
            Contract.Requires(settingsProvider != null);

            this.settingsProvider = settingsProvider;
        }

        public async Task<SettingsCollection> Handle(AllSettingsQuery request)
        {
            return await Task<SettingsCollection>.Factory.StartNew(() => this.settingsProvider.GetSettings());
        }

        public async Task<Setting> Handle(SettingByNameRequest request)
        {
            return await Task<Setting>.Factory.StartNew(() => this.settingsProvider.GetSettings()[request.Name]);
        }
    }
}