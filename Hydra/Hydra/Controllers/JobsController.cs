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
            IEnumerable<JobInfo> availableJobs = this.mediator.Request(new AvailableJobs());

            return this.View(availableJobs);
        }

        public ActionResult Schedule(JobInfo info)
        {
            return this.View(info);
        }
    }
}