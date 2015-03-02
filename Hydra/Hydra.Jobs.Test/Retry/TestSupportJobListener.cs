namespace Hydra.Jobs.Test.Retry
{
    using System.Threading;
    using Quartz;

    public class TestSupportJobListener : IJobListener
    {
        private readonly IJobListener inner;

        private readonly ManualResetEvent reset;

        private int counter;

        public TestSupportJobListener(IJobListener inner, ManualResetEvent reset)
        {
            this.inner = inner;
            this.reset = reset;
            this.counter = 0;
        }

        public string Name
        {
            get { return this.inner.Name; }
        }

        public void JobToBeExecuted(IJobExecutionContext context)
        {
            this.inner.JobToBeExecuted(context);
        }

        public void JobExecutionVetoed(IJobExecutionContext context)
        {
            this.inner.JobExecutionVetoed(context);
        }

        public void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
            this.inner.JobWasExecuted(context, jobException);

            if (JobFailed(jobException))
            {
                this.counter++;

                if (this.counter >= 3)
                {
                    this.reset.Set();
                }
            }
        }

        private static bool JobFailed(JobExecutionException jobException)
        {
            return jobException != null;
        }
    }
}