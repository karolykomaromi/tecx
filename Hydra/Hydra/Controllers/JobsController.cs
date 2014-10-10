namespace Hydra.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Web.Mvc;
    using Hydra.Models;
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
            IEnumerable<IJobDetail> scheduledJobs = this.mediator.Request(new ScheduledJobs());

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

            ScheduleNewJob newJob = new ScheduleNewJob { };

            return this.View(newJob);
        }

        [HttpPost]
        public ActionResult Schedule(ScheduleNewJob scheduleNewJob)
        {
            return this.View();
        }
    }
}