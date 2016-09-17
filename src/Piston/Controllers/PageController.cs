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

        [Route("~/pages/{slug}", Name = "Page")]
        public ActionResult Index(string slug)
        {
            var page = _pageStorage.GetAllPages().SingleOrDefault(p => p.Url == slug);

            if (page == null)
            {
                return HttpNotFound();
            }

            return View(page);
        }
    }
}