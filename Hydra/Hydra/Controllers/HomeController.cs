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
            HomeModel model = new HomeModel { Foo = "Bar" };

            return this.View(model);
        }
    }
}