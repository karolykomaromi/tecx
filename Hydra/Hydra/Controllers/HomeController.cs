namespace Hydra.Controllers
{
    using System.Web.Mvc;
    using Hydra.Features.Registration;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            RegistrationViewModel model = new RegistrationViewModel { Email = "foo@bar.local" };

            return this.View(model);
        }
    }
}