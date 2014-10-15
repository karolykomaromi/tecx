using Hydra.Features.Registration;

namespace Hydra.Controllers
{
    using System.Web.Mvc;

    public class RegistrationController : Controller
    {
        public ActionResult Register()
        {
            return this.View(new Registration());
        }

        [HttpPost]
        public ActionResult Register(Registration registration)
        {
            return this.RedirectToAction("Index", "Home");
        }
    }
}