using Piston.Models;
using System.Collections.Generic;

namespace Piston.Storage
{
    public interface IPostStorage
    {
        IEnumerable<Post> GetAllPosts();
    }
}