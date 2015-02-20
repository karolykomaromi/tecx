namespace Hydra.Jobs.Server.Jobs
{
    using System.Net;

    public class MailSettings
    {
        public virtual ICredentialsByHost Credentials { get; set; }

        public virtual int Port { get; set; }

        public virtual string Host { get; set; }
    }
}