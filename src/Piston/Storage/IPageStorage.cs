namespace Piston.Storage
{
    using Models;
    using System.Collections.Generic;

    public interface IPageStorage
    {
        IEnumerable<Page> GetAllPages();
    }
}
