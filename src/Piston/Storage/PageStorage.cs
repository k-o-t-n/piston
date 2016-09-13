using System.Collections.Generic;
using System.Linq;
using Piston.Models;
using HeyRed.MarkdownSharp;
using System.Web.Hosting;
using System.IO;

namespace Piston.Storage
{
    internal class PageStorage : IPageStorage
    {
        private static string _pages = HostingEnvironment.MapPath("~/pages/");
        private readonly IDirectoryReader _directoryReader;
        private readonly Markdown _mark;

        public PageStorage(Markdown mark, IDirectoryReader directoryReader)
        {
            _mark = mark;
            _directoryReader = directoryReader;
        }

        public IEnumerable<Page> GetAllPages()
        {
            var pages = new List<Page>();

            foreach (var file in _directoryReader.EnumerateFiles(_pages))
            {
                var page = new Page
                {
                    Title = Path.GetFileNameWithoutExtension(file.FileName),
                    Content = _mark.Transform(file.Body)
                };

                pages.Add(page);
            }

            return pages.AsEnumerable();
        }
    }
}