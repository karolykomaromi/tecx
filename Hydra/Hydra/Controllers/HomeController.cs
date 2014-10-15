using Hydra.Features.Registration;

namespace Hydra.Controllers
{
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Registration model = new Registration { Email = "foo@bar.local" };

            return this.View(model);
        }
    }
}