namespace Hydra.Controllers
{
    using System.Web.Mvc;

    public class CoolingController : Controller
    {
        public ActionResult Dashboard()
        {
            return this.View();
        }
    }
}