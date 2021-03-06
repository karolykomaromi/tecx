namespace Hydra.Jobs.Server.Jobs
{
    using System;
    using System.Diagnostics.Contracts;
    using Quartz;

    public class EnsureJobExecutionExceptionDecorator : IJob
    {
        private readonly IJob inner;

        public EnsureJobExecutionExceptionDecorator(IJob inner)
        {
            Contract.Requires(inner != null);

            this.inner = inner;
        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                this.inner.Execute(context);
            }
            catch (JobExecutionException)
            {
                // JobExecutionExceptions are handled by Quartz
                throw;
            }
            catch (Exception cause)
            {
                // No other exception Type is allowed so we wrap them in a JobExecutionException (which is handled by Quartz)
                throw new JobExecutionException(cause);
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.inner != null);
        }
    }
}