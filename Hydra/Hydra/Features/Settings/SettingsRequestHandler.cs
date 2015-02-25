namespace Hydra.Features.Settings
{
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using Hydra.Infrastructure.Configuration;
    using Hydra.Infrastructure.Mediator;

    public class SettingsRequestHandler : IRequestHandler<AllSettingsRequest, SettingsCollection>, IRequestHandler<SettingByNameRequest, Setting>
    {
        private readonly ISettingsProvider settingsProvider;

        public SettingsRequestHandler(ISettingsProvider settingsProvider)
        {
            Contract.Requires(settingsProvider != null);

            this.settingsProvider = settingsProvider;
        }

        public async Task<SettingsCollection> Handle(AllSettingsRequest request)
        {
            Contract.Requires(request != null);

            return await Task<SettingsCollection>.Factory.StartNew(() => this.settingsProvider.GetSettings());
        }

        public async Task<Setting> Handle(SettingByNameRequest request)
        {
            Contract.Requires(request != null);
            Contract.Requires(request.Name != null);

            return await Task<Setting>.Factory.StartNew(() => this.settingsProvider.GetSettings()[request.Name]);
        }
    }
}