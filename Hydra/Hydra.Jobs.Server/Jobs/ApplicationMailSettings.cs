namespace Hydra.Jobs.Server.Jobs
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Net;
    using Hydra.Infrastructure.Configuration;

    public class ApplicationMailSettings : IMailJobSettings
    {
        private readonly ISettingsProvider settingsProvider;

        private readonly Lazy<ICredentials> credentials;

        private readonly Lazy<int> port;

        private readonly Lazy<string> host;

        private readonly Lazy<bool> isAuthenticationRequired;

        public ApplicationMailSettings(ISettingsProvider settingsProvider)
        {
            Contract.Requires(settingsProvider != null);

            this.settingsProvider = settingsProvider;

            this.isAuthenticationRequired = new Lazy<bool>(() => (bool)this.settingsProvider.GetSettings()[KnownSettingNames.Hydra.Mail.Smtp.IsAuthenticationRequired].Value);

            this.credentials = new Lazy<ICredentials>(
                () =>
                {
                        string userName = (string)this.settingsProvider.GetSettings()[KnownSettingNames.Hydra.Mail.Smtp.UserName].Value;

                        string password = (string)this.settingsProvider.GetSettings()[KnownSettingNames.Hydra.Mail.Smtp.Password].Value;

                        return new NetworkCredential(userName, password);
                });

            this.port = new Lazy<int>(() => (int)this.settingsProvider.GetSettings()[KnownSettingNames.Hydra.Mail.Smtp.Port].Value);

            this.host = new Lazy<string>(() => (string)this.settingsProvider.GetSettings()[KnownSettingNames.Hydra.Mail.Smtp.Host].Value);
        }
        
        public ICredentials Credentials
        {
            get { return this.credentials.Value; }
        }

        public int Port
        {
            get { return this.port.Value; }
        }

        public string Host
        {
            get { return this.host.Value; }
        }

        public bool IsAuthenticationRequired
        {
            get { return this.isAuthenticationRequired.Value; }
        }
    }
}