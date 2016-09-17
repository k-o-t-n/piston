namespace Piston.Controllers
{
    using System.Web.Mvc;

    public class ErrorsController : Controller
    {
        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public ActionResult NotFound()
        {
            return View();
        }

        [HttpGet]
        public ActionResult InternalError()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}