namespace Hydra.Jobs.Server
{
    using Quartz;

    public class NoOp : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
        }
    }
}