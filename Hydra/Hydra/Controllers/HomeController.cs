namespace Hydra.Controllers
{
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        // GET: /Home/
        public ActionResult Index()
        {
            return this.Content("Hello World!");
        }
    }
}