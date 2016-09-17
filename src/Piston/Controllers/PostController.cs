namespace Piston.Controllers
{
    using Models;
    using Storage;
    using System;
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
        [Route("~/page/{page:int}")]
        [Route("~/category/{category}", Name = "Category")]
        [Route("~/category/{category}/page/{page:int}")]
        [Route("~/tag/{tag}", Name = "Tag")]
        [Route("~/tag/{tag}/page/{page:int}")]
        public ActionResult Index(int page = 1, string category = null, string tag = null)
        {
            var posts = _postStorage.GetAllPosts()
                .Where(p => p.IsPublished && p.Date <= DateTime.Now.Date
                    && (string.IsNullOrWhiteSpace(category) || p.Categories.Contains(category))
                    && (string.IsNullOrWhiteSpace(tag) || p.Tags.Contains(tag)));

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

            return View(post.Layout, post);
        }
    }
}