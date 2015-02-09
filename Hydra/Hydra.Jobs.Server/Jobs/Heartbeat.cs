namespace Hydra.Jobs.Server.Jobs
{
    using Quartz;

    public class Heartbeat : IJob
    {
        public Heartbeat()
        {
            // weberse 2015-02-09 use signalr to notify the clients that we are still here
        }

        public void Execute(IJobExecutionContext context)
        {
        }
    }
}
