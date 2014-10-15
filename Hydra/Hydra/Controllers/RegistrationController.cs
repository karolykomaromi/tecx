namespace Hydra.Controllers
{
    using System.Web.Mvc;
    using Hydra.Features.Registration;

    public class RegistrationController : Controller
    {
        public ActionResult Register()
        {
            return this.View(new RegistrationViewModel());
        }

        [HttpPost]
        public ActionResult Register(RegistrationViewModel registration)
        {
            return this.RedirectToAction("Index", "Home");
        }
    }
}