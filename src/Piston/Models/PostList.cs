namespace Piston.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class PostList
    {
        private readonly IEnumerable<Post> _posts;

        public PostList(IEnumerable<Post> posts)
        {
            _posts = posts;
            Page = 1;
        }

        public IEnumerable<Post> Posts
        {
            get
            {
                return _posts.Skip(Settings.PostsPerPage * (Page - 1)).Take(Settings.PostsPerPage);
            }
        }

        public int Page { get; set; }

        public int PageCount
        {
            get
            {
                return (_posts.Count() + Settings.PostsPerPage - 1) / Settings.PostsPerPage;
            }
        }

        public int PostCount
        {
            get
            {
                return _posts.Count();
            }
        }
    }
}