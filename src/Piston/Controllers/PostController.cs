using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Piston.Controllers
{
    public class PostController : Controller
    {
        [Route("~/post/{*slug}", Name = "Post")]
        public ActionResult Index(string slug)
        {
            var post = Storage.GetAllPosts().SingleOrDefault(p => p.Url == slug);

            if (post == null)
            {
                return HttpNotFound();
            }

            return View(post);
        }
    }
}