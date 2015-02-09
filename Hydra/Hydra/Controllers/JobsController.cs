namespace Hydra.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Hydra.Features.Jobs;
    using Hydra.Jobs.Client;
    using Hydra.Queries;
    using Quartz;

    public class JobsController : Controller
    {
        private readonly IMediator mediator;
        private readonly ISchedulerClient proxy;

        public JobsController(IMediator mediator, ISchedulerClient proxy)
        {
            Contract.Requires(mediator != null);
            Contract.Requires(proxy != null);

            this.mediator = mediator;
            this.proxy = proxy;
        }

        public ActionResult Index()
        {
            IEnumerable<IJobDetail> scheduledJobs = this.mediator.Query(new ScheduledJobsQuery());

            return this.View(scheduledJobs);
        }

        public ActionResult Delete(JobKey key)
        {
            return this.RedirectToAction("Index");
        }

        public ActionResult Schedule()
        {
            ScheduleViewModel newJob = new ScheduleViewModel { };

            return this.View(newJob);
        }

        [HttpPost]
        public async Task<ActionResult> Schedule(ScheduleViewModel scheduleNewJob)
        {
            SimpleJobScheduleRequest request = new SimpleJobScheduleRequest();

            JobScheduleResponse response = await this.proxy.Schedule(request);

            return this.View();
        }
    }
}