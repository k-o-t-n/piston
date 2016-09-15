namespace Piston.Controllers
{
    using Storage;
    using System.Linq;
    using System.Web.Mvc;

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