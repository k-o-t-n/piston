using System.Collections.Generic;

namespace Piston.Storage
{
    public interface IDirectoryReader
    {
        IEnumerable<RawContent> EnumerateFiles(string path);
    }
}
