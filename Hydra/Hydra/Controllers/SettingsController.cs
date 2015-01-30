namespace Hydra.Controllers
{
    using System.Diagnostics.Contracts;
    using System.Web.Mvc;
    using Hydra.Infrastructure.Configuration;

    public class SettingsController : Controller
    {
        private readonly ISettingsProvider settingsProvider;

        public SettingsController(ISettingsProvider settingsProvider)
        {
            Contract.Requires(settingsProvider != null);

            this.settingsProvider = settingsProvider;
        }

        public ActionResult Index()
        {
            var settings = this.settingsProvider.GetSettings();

            return this.View(settings);
        }

        public ActionResult Edit(string settingName)
        {
            Contract.Requires(settingName != null);

            SettingName sn;
            if (!SettingName.TryParse(settingName, out sn))
            {
                
            }

            Setting setting = this.settingsProvider.GetSettings()[sn];

            return this.View(setting);
        }
	}
}