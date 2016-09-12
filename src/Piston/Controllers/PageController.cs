using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Piston.Controllers
{
    public class PageController : Controller
    {
        [Route("~/page/{pageName}", Name = "Page")]
        public ActionResult Index(string pageName)
        {
            var page = Storage.GetAllPages().SingleOrDefault(p => p.Title == pageName);

            if (page == null)
            {
                return HttpNotFound();
            }

            return View(page);
        }
    }
}