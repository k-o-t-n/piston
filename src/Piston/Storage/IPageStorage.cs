using Piston.Models;
using System.Collections.Generic;

namespace Piston.Storage
{
    public interface IPageStorage
    {
        IEnumerable<Page> GetAllPages();
    }
}
