namespace Hydra.Jobs.Server
{
    using System.Diagnostics.Contracts;
    using System.ServiceModel;

    [ServiceContract(Namespace = Constants.ServiceNamespace)]
    [ContractClass(typeof(SchedulerServiceContract))]
    public interface ISchedulerService
    {
        [OperationContract(Name = Constants.Methods.SimpleSchedule)]
        JobScheduleResponse Schedule(SimpleJobScheduleRequest jobSchedule);
    }

    [ContractClassFor(typeof(ISchedulerService))]
    internal abstract class SchedulerServiceContract : ISchedulerService
    {
        public JobScheduleResponse Schedule(SimpleJobScheduleRequest jobSchedule)
        {
            Contract.Requires(jobSchedule != null);
            Contract.Ensures(Contract.Result<JobScheduleResponse>() != null);

            return new JobScheduleResponse();
        }
    }
}
