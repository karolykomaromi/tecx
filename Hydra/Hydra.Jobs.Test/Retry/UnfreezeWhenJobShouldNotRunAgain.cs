namespace Hydra.Jobs.Test.Retry
{
    using System.Threading;
    using Hydra.Jobs.Server.Retry;
    using Quartz;

    public class UnfreezeWhenJobShouldNotRunAgain : IRetryStrategy
    {
        private readonly IRetryStrategy inner;
        private readonly ManualResetEvent reset;

        public UnfreezeWhenJobShouldNotRunAgain(IRetryStrategy inner, ManualResetEvent reset)
        {
            this.inner = inner;
            this.reset = reset;
        }

        public bool ShouldRetry(IJobExecutionContext context)
        {
            bool shouldRetry = this.inner.ShouldRetry(context);

            if (!shouldRetry)
            {
                this.reset.Set();
            }

            return shouldRetry;
        }

        public ITrigger GetTrigger(IJobExecutionContext context)
        {
            return this.inner.GetTrigger(context);
        }
    }
}