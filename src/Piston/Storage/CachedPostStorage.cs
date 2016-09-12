using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Piston.Models;

namespace Piston.Storage
{
    internal class CachedPostStorage : IPostStorage
    {
        private readonly IPostStorage _inner;

        public CachedPostStorage(IPostStorage inner)
        {
            _inner = inner;
        }

        public IEnumerable<Post> GetAllPosts()
        {
            return CacheHelper.GetOrCache("posts", _inner.GetAllPosts);
        }
    }
}