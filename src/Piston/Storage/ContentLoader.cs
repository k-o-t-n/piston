using System;
using System.IO;

namespace Piston.Storage
{
    internal class ContentLoader : IContentLoader
    {
        private readonly IFileReader _fileReader;

        public ContentLoader(IFileReader fileReader)
        {
            _fileReader = fileReader;
        }

        public RawContent ReadFile(string filePath)
        {
            var rawContent = _fileReader.ReadAllText(filePath);
            var fileName = Path.GetFileName(filePath);

            var startOfSettingsIndex = rawContent.IndexOf("---", StringComparison.InvariantCultureIgnoreCase);
            if (startOfSettingsIndex >= 0)
            {
                var endOfSettingsIndex = rawContent.IndexOf("---", startOfSettingsIndex + 3,
                    StringComparison.InvariantCultureIgnoreCase);

                if (endOfSettingsIndex >= 0)
                {
                    var parsedSettings = rawContent.Substring(startOfSettingsIndex, endOfSettingsIndex + 3);
                    var parsedContent = rawContent.Substring(endOfSettingsIndex + 3, rawContent.Length - (endOfSettingsIndex + 3));

                    return new RawContent
                    {
                        FileName = fileName,
                        Header = parsedSettings,
                        Body = parsedContent
                    };
                }
            }

            return new RawContent
            {
                FileName = fileName,
                Body = rawContent
            };
        }
    }
}