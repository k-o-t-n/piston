using System.Collections.Generic;
using System.IO;

namespace Piston.Storage
{
    internal class DirectoryReader : IDirectoryReader
    {
        private readonly IContentLoader _contentLoader;

        public DirectoryReader(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        public IEnumerable<RawContent> EnumerateFiles(string path)
        {
            foreach (var file in Directory.EnumerateFiles(path, "*.md", SearchOption.TopDirectoryOnly))
            {
                yield return _contentLoader.ReadFile(file);
            }
        }
    }
}