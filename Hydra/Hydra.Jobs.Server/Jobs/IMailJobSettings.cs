namespace Hydra.Jobs.Server.Jobs
{
    using System.Net;

    public interface IMailJobSettings
    {
        ICredentials Credentials { get; }

        int Port { get; }

        string Host { get; }

        bool IsAuthenticationRequired { get; }
    }
}