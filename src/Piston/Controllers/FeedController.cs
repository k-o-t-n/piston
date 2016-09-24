namespace Piston.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.ServiceModel.Syndication;
    using Storage;
    using System.Linq;

    public class FeedController : Controller
    {
        private readonly IPostStorage _postStorage;

        public FeedController(IPostStorage postStorage)
        {
            _postStorage = postStorage;
        }

        [OutputCache(Duration = 86400)]
        [Route("~/feed/atom", Name = "AtomFeed")]
        public ActionResult Atom()
        {
            var baseUri = new Uri(HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Authority);

            var feed = new SyndicationFeed
            {
                Title = new TextSyndicationContent(Settings.Title),
                Description = new TextSyndicationContent("Latest posts"),
                BaseUri = baseUri,
                Id = baseUri.ToString(),
                Items = GetItems(baseUri)
            };

            return new AtomActionResult(feed);
        }

        private IEnumerable<SyndicationItem> GetItems(Uri baseUri)
        {
            foreach (var post in _postStorage.GetAllPosts().Take(Settings.PostsPerPage))
            {
                var postUri = new Uri(baseUri, Url.RouteUrl("Post", new { slug = post.Url }));

                var item = new SyndicationItem(post.Title, SyndicationContent.CreateHtmlContent(post.Content), 
                    postUri, postUri.ToString(), post.Date)
                {
                    PublishDate = post.Date
                };

                item.Summary = SyndicationContent.CreateHtmlContent(post.Preview);

                item.Authors.Add(new SyndicationPerson(string.Empty, post.Author, string.Empty));

                yield return item;
            }
        }
    }
}