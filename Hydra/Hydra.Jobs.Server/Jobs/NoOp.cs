namespace Hydra.Jobs.Server.Jobs
{
    using Quartz;

    public class NoOp : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            /* intentionally left blank */
        }
    }
}