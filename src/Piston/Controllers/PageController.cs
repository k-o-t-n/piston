using Piston.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Piston.Controllers
{
    public class PageController : Controller
    {
        private readonly IPageStorage _pageStorage;

        public PageController(IPageStorage pageStorage)
        {
            _pageStorage = pageStorage;
        }

        [Route("~/page/{pageName}", Name = "Page")]
        public ActionResult Index(string pageName)
        {
            var page = _pageStorage.GetAllPages().SingleOrDefault(p => p.Title == pageName);

            if (page == null)
            {
                return HttpNotFound();
            }

            return View(page);
        }

        [Route("~/page/menu")]
        public ActionResult Menu()
        {
            var pages = _pageStorage.GetAllPages();

            return PartialView(pages);
        }
    }
}