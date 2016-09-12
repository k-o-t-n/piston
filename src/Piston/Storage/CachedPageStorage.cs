using System.Collections.Generic;
using Piston.Models;

namespace Piston.Storage
{
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