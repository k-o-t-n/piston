namespace Piston.Controllers
{
    using Models;
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
        [Route("~/page/{page:int?}")]
        public ActionResult Index(int page = 1)
        {
            var posts = _postStorage.GetAllPosts();

            var model = new PostList(posts)
            {
                Page = page
            };

            return View(model);
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