namespace Hydra.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Hydra.Features.Jobs;
    using Hydra.Queries;
    using Quartz;

    public class JobsController : Controller
    {
        private readonly IMediator mediator;

        public JobsController(IMediator mediator)
        {
            Contract.Requires(mediator != null);

            this.mediator = mediator;
        }

        public async Task<ActionResult> Index()
        {
            IEnumerable<IJobDetail> scheduledJobs = await this.mediator.Query(new ScheduledJobsQuery());

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
            var result = await this.mediator.Send(new ScheduleJobCommand { Job = scheduleNewJob.Job });

            return this.View();
        }
    }
}