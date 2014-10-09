namespace Hydra.Controllers
{
    using System.Diagnostics.Contracts;
    using System.Web.Mvc;
    using Quartz;

    public class JobsController : Controller
    {
        private readonly IScheduler scheduler;

        public JobsController(IScheduler scheduler)
        {
            Contract.Requires(scheduler != null);

            this.scheduler = scheduler;
        }

        public ActionResult Index()
        {
            return this.View();
        }
    }
}