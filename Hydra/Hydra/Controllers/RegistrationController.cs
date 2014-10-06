namespace Hydra.Controllers
{
    using System.Web.Mvc;
    using Hydra.Models;

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