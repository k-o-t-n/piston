namespace Piston.Storage
{
    using System.Collections.Generic;
    using Models;

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