namespace Hydra.Controllers
{
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Hydra.Features.Settings;
    using Hydra.Infrastructure.Configuration;
    using Hydra.Infrastructure.Mediator;

    public class SettingsController : Controller
    {
        private readonly IMediator mediator;

        public SettingsController(IMediator mediator)
        {
            Contract.Requires(mediator != null);

            this.mediator = mediator;
        }

        public async Task<ActionResult> Index()
        {
            SettingsCollection settings = await this.mediator.Send(new AllSettingsRequest());

            return this.View(settings);
        }

        public async Task<ActionResult> Edit(string settingName)
        {
            Contract.Requires(settingName != null);

            SettingName sn;
            if (!SettingName.TryParse(settingName, out sn))
            {
                return await this.Index();
            }

            Setting setting = await this.mediator.Send(new SettingByNameRequest { Name = sn });

            return this.View(setting);
        }
    }
}