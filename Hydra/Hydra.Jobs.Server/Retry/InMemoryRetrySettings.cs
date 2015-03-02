namespace Hydra.Jobs.Server.Retry
{
    using System;
    using Hydra.Infrastructure;

    public class InMemoryRetrySettings : IRetrySettings
    {
        public InMemoryRetrySettings()
        {
            this.MaxRetries = 3;
            this.BackoffBaseInterval = 2.Minutes();
        }

        public int MaxRetries { get; set; }

        public TimeSpan BackoffBaseInterval { get; set; }
    }
}