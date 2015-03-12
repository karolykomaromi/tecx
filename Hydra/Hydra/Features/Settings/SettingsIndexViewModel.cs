namespace Hydra.Features.Settings
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Hydra.Infrastructure.Configuration;

    public class SettingsIndexViewModel
    {
        private readonly SettingsCollection settings;

        public SettingsIndexViewModel(SettingsCollection settings)
        {
            Contract.Requires(settings != null);

            this.settings = settings;
        }

        public SettingsCollection Settings
        {
            get { return this.settings; }
        }
    }
}