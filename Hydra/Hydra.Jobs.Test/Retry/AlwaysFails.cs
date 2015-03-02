namespace Hydra.Jobs.Test.Retry
{
    using System;
    using Quartz;

    public class AlwaysFails : IJob
    {
        private int counter;

        public AlwaysFails()
        {
            this.counter = 0;
        }

        public int Counter
        {
            get { return this.counter; }
        }

        public void Execute(IJobExecutionContext context)
        {
            this.counter++;

            throw new NotImplementedException();
        }
    }
}
