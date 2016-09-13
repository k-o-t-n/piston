using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Piston.Models;
using System.IO;
using System.Web.Hosting;
using System.Text.RegularExpressions;
using System.Globalization;
using HeyRed.MarkdownSharp;

namespace Piston.Storage
{
    internal class PostStorage : IPostStorage
    {
        private static string _posts = HostingEnvironment.MapPath("~/posts/");
        private static readonly Regex FileNameRegex =
            new Regex(@"^(?<year>\d{4})-(?<month>\d{2})-(?<day>\d{2})-(?<slug>.+).(?:md|markdown)$",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Markdown _mark;
        private readonly IDirectoryReader _directoryReader;

        public PostStorage(Markdown mark, IDirectoryReader directoryReader)
        {
            _mark = mark;
            _directoryReader = directoryReader;
        }

        public IEnumerable<Post> GetAllPosts()
        {
            var posts = new List<Post>();

            foreach (var file in _directoryReader.EnumerateFiles(_posts))
            {
                var fileNameMatches = FileNameRegex.Match(file.FileName);

                if (!fileNameMatches.Success)
                {
                    System.Diagnostics.Debug.WriteLine($"Skipping file {file.FileName}");
                    break;
                }

                var year = fileNameMatches.Groups["year"].Value;
                var month = fileNameMatches.Groups["month"].Value;
                var day = fileNameMatches.Groups["day"].Value;
                var slug = fileNameMatches.Groups["slug"].Value.ToUrlSlug();
                var date = DateTime.ParseExact(year + month + day, "yyyyMMdd", CultureInfo.InvariantCulture);

                var post = new Post
                {
                    FileName = file.FileName,
                    Content = _mark.Transform(file.Body),
                    Year = date.Year,
                    Month = date.Month,
                    Day = date.Day,
                    Date = date,
                    Url = slug
                };

                post.SetDefaultSettings();
                post.SetHeaderSettings(ParseSettings(file.Header));

                posts.Add(post);
            }

            posts.SetPostUrl();

            return posts.AsEnumerable();
        }

        private static Dictionary<string, object> ParseSettings(string rawSettings)
        {
            if (string.IsNullOrWhiteSpace(rawSettings))
            {
                return new Dictionary<string, object>();
            }

            rawSettings = rawSettings.Trim('-');

            var lines = rawSettings.Split(new[] { "\n", "\r", "\n\r" }, StringSplitOptions.RemoveEmptyEntries);
            var result = new Dictionary<string, object>();

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                var setting = line.Split(new[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries);
                result.Add(setting[0].Trim(), setting[1].Trim());
            }

            return result;
        }
    }
}