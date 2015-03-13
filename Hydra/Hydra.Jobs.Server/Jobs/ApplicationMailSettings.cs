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

            this.isAuthenticationRequired = new Lazy<bool>(() => this.settingsProvider.GetValue<bool>(KnownSettingNames.Hydra.Mail.Smtp.IsAuthenticationRequired));

            this.credentials = new Lazy<ICredentials>(
                () =>
                {
                        string userName = this.settingsProvider.GetValue<string>(KnownSettingNames.Hydra.Mail.Smtp.UserName);

                        string password = this.settingsProvider.GetValue<string>(KnownSettingNames.Hydra.Mail.Smtp.Password);

                        return new NetworkCredential(userName, password);
                });

            this.port = new Lazy<int>(() => this.settingsProvider.GetValue<int>(KnownSettingNames.Hydra.Mail.Smtp.Port));

            this.host = new Lazy<string>(() => this.settingsProvider.GetValue<string>(KnownSettingNames.Hydra.Mail.Smtp.Host));
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

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.credentials != null);
            Contract.Invariant(this.port != null);
            Contract.Invariant(this.host != null);
            Contract.Invariant(this.isAuthenticationRequired != null);
        }
    }
}