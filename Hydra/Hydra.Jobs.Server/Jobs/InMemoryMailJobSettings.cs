namespace Hydra.Jobs.Server.Jobs
{
    using System.Net;

    public class InMemoryMailJobSettings : IMailJobSettings
    {
        public ICredentials Credentials { get; set; }

        public int Port { get; set; }

        public string Host { get; set; }

        public bool IsAuthenticationRequired { get; set; }
    }
}