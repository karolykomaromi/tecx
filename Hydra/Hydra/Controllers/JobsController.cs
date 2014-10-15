namespace Hydra.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Web.Mvc;
    using Hydra.Features.Jobs;
    using Hydra.Queries;
    using Quartz;

    public class JobsController : Controller
    {
        private readonly IScheduler scheduler;
        private readonly IMediator mediator;

        public JobsController(IScheduler scheduler, IMediator mediator)
        {
            Contract.Requires(scheduler != null);
            Contract.Requires(mediator != null);

            this.scheduler = scheduler;
            this.mediator = mediator;
        }

        public ActionResult Index()
        {
            IEnumerable<IJobDetail> scheduledJobs = this.mediator.Request(new ScheduledJobsQuery());

            return this.View(scheduledJobs);
        }

        public ActionResult Delete(JobKey key)
        {
            return this.RedirectToAction("Index");
        }

        public ActionResult Schedule()
        {
            ////IJobDetail job = JobBuilder.Create<SendEmails>().WithIdentity("foobar").WithDescription(info.Description).Build();

            ////ITrigger trigger = TriggerBuilder.Create().ForJob(job).WithCalendarIntervalSchedule(c => c.)

            ////this.scheduler.ScheduleJob(job);

            ScheduleViewModel newJob = new ScheduleViewModel { };

            return this.View(newJob);
        }

        [HttpPost]
        public ActionResult Schedule(ScheduleViewModel scheduleNewJob)
        {
            return this.View();
        }
    }
}