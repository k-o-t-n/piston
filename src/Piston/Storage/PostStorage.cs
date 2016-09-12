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

        public PostStorage(Markdown mark)
        {
            _mark = mark;
        }

        public IEnumerable<Post> GetAllPosts()
        {
            var posts = new List<Post>();

            foreach (string file in Directory.EnumerateFiles(_posts, "*.md", SearchOption.TopDirectoryOnly))
            {
                var fileName = Path.GetFileName(file);
                var fileNameMatches = FileNameRegex.Match(fileName);

                if (!fileNameMatches.Success)
                {
                    System.Diagnostics.Debug.WriteLine($"Skipping file {fileName}");
                    break;
                }

                var year = fileNameMatches.Groups["year"].Value;
                var month = fileNameMatches.Groups["month"].Value;
                var day = fileNameMatches.Groups["day"].Value;
                var slug = fileNameMatches.Groups["slug"].Value.ToUrlSlug();
                var date = DateTime.ParseExact(year + month + day, "yyyyMMdd", CultureInfo.InvariantCulture);

                var rawContent = File.ReadAllText(file);
                Dictionary<string, object> settings = new Dictionary<string, object>();
                string bodySerialized = string.Empty;

                var startOfSettingsIndex = rawContent.IndexOf("---", StringComparison.InvariantCultureIgnoreCase);
                if (startOfSettingsIndex >= 0)
                {
                    //Find the second index of --- after the first
                    var endOfSettingsIndex = rawContent.IndexOf("---", startOfSettingsIndex + 3,
                        StringComparison.InvariantCultureIgnoreCase);

                    //If we find the 2nd index, parse the settings
                    //Otherwise we assume there's no header or settings...
                    if (endOfSettingsIndex >= 0)
                    {
                        var parsedSettings = rawContent.Substring(startOfSettingsIndex, endOfSettingsIndex + 3);
                        var parsedContent = rawContent.Substring(endOfSettingsIndex + 3, rawContent.Length - (endOfSettingsIndex + 3));
                        bodySerialized = _mark.Transform(parsedContent);

                        settings = ParseSettings(parsedSettings);
                    }
                }
                else
                {
                    bodySerialized = _mark.Transform(rawContent);
                }

                var post = new Post
                {
                    FileName = fileName,
                    Content = bodySerialized,
                    Settings = settings,
                    Year = date.Year,
                    Month = date.Month,
                    Day = date.Day,
                    Date = date,
                    Url = slug
                };

                post.SetDefaultSettings();
                post.SetHeaderSettings(settings);

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