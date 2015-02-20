namespace Hydra.Jobs.Server.Jobs
{
    using Quartz;

    public class Nop : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            /* intentionally left blank */
        }
    }
}