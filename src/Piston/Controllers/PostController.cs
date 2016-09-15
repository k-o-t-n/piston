namespace Piston.Controllers
{
    using Storage;
    using System.Linq;
    using System.Web.Mvc;

    public class PostController : Controller
    {
        private readonly IPostStorage _postStorage;

        public PostController(IPostStorage postStorage)
        {
            _postStorage = postStorage;
        }

        [Route("~/", Name = "Home")]
        public ActionResult Index()
        {
            var posts = _postStorage.GetAllPosts();

            return View(posts);
        }

        [Route("~/post/{*slug}", Name = "Post")]
        public ActionResult ViewPost(string slug)
        {
            var post = _postStorage.GetAllPosts().SingleOrDefault(p => p.Url == slug);

            if (post == null)
            {
                return HttpNotFound();
            }

            return View(post);
        }
    }
}