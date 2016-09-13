using System.IO;

namespace Piston.Storage
{
    internal class FileReader : IFileReader
    {
        public string ReadAllText(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}