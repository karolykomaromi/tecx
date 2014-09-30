namespace Hydra.Controllers
{
    using System.Web.Mvc;
    using Hydra.Models;

    public class HomeController : Controller
    {
        // GET: /Home/
        public ActionResult Index()
        {
            // return this.Content("Hello World!");
            Registration model = new Registration { Email = "foo@bar.local" };

            return this.View(model);
        }
    }
}