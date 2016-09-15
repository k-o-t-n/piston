namespace Piston.Storage
{
    using System.Collections.Generic;
    using System.Linq;
    using Models;
    using System.Web.Hosting;
    using System.IO;
    using Markdown;

    internal class LocalPageStorage : IPageStorage
    {
        private static readonly string _pages = HostingEnvironment.MapPath("~/pages/");
        private readonly MetadataMarkdown _mark;

        public LocalPageStorage(MetadataMarkdown mark)
        {
            _mark = mark;
        }

        public IEnumerable<Page> GetAllPages()
        {
            var pages = new List<Page>();

            foreach (var filePath in Directory.EnumerateFiles(_pages))
            {
                var fileContent = File.ReadAllText(filePath);

                var page = new Page
                {
                    Title = Path.GetFileNameWithoutExtension(filePath),
                    Content = _mark.Transform(fileContent)
                };

                pages.Add(page);
            }

            return pages.AsEnumerable();
        }
    }
}