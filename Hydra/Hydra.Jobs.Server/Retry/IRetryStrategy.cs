namespace Hydra.Jobs.Server.Retry
{
    using Quartz;

    public interface IRetryStrategy
    {
        bool ShouldRetry(IJobExecutionContext context);

        ITrigger GetTrigger(IJobExecutionContext context);
    }
}