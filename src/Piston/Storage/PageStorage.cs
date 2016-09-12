using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Piston.Models;
using System.IO;
using HeyRed.MarkdownSharp;
using System.Web.Hosting;

namespace Piston.Storage
{
    internal class PageStorage : IPageStorage
    {
        private static string _pages = HostingEnvironment.MapPath("~/pages/");
        private readonly Markdown _mark;

        public PageStorage(Markdown mark)
        {
            _mark = mark;
        }

        public IEnumerable<Page> GetAllPages()
        {
            var pages = new List<Page>();

            foreach (string file in Directory.EnumerateFiles(_pages, "*.md", SearchOption.TopDirectoryOnly))
            {
                var page = new Page
                {
                    Title = Path.GetFileNameWithoutExtension(file),
                    Content = _mark.Transform(File.ReadAllText(file))
                };

                pages.Add(page);
            }

            return pages.AsEnumerable();
        }
    }
}