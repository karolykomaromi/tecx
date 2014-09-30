namespace Hydra.Controllers
{
    using System.Web.Mvc;
    using Hydra.Models;

    public class RegistrationController : Controller
    {
        public ActionResult Create()
        {
            return this.View(new Registration());
        }

        public ActionResult Save(Registration registration)
        {
            return this.Content("Successfully registered.");
        }
    }
}