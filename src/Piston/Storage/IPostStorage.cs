namespace Piston.Storage
{
    using Models;
    using System.Collections.Generic;

    public interface IPostStorage
    {
        IEnumerable<Post> GetAllPosts();
    }
}