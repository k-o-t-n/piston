namespace Piston.Storage
{
    using System.Collections.Generic;
    using Models;

    internal class CachedPageStorage : IPageStorage
    {
        private readonly IPageStorage _inner;

        public CachedPageStorage(IPageStorage inner)
        {
            _inner = inner;
        }

        public IEnumerable<Page> GetAllPages()
        {
            return CacheHelper.GetOrCache("pages", _inner.GetAllPages);
        }
    }
}