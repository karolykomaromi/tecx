namespace Hydra.Jobs.Server.Retry
{
    using System;

    public interface IRetrySettings
    {
        int MaxRetries { get; }

        TimeSpan BackoffBaseInterval { get; }
    }
}