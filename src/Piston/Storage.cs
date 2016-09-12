using HeyRed.MarkdownSharp;
using Piston.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;

namespace Piston
{
    public static class Storage
    {
        private static string _pages = HostingEnvironment.MapPath("~/pages/");
        private static string _posts = HostingEnvironment.MapPath("~/posts/");
        private static readonly Regex FileNameRegex =
            new Regex(@"^(?<year>\d{4})-(?<month>\d{2})-(?<day>\d{2})-(?<slug>.+).(?:md|markdown)$",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Markdown _mark = new Markdown();

        public static List<Page> GetAllPages()
        {
            if (HttpRuntime.Cache["pages"] == null)
            {
                LoadPages();
            }

            if (HttpRuntime.Cache["pages"] != null)
            {
                return (List<Page>)HttpRuntime.Cache["pages"];
            }

            return new List<Page>();
        }

        private static void LoadPages()
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

            HttpRuntime.Cache.Insert("pages", pages);
        }

        public static List<Post> GetAllPosts()
        {
            if (HttpRuntime.Cache["posts"] == null)
            {
                LoadPosts();
            }

            if (HttpRuntime.Cache["posts"] != null)
            {
                return (List<Post>)HttpRuntime.Cache["posts"];
            }

            return new List<Post>();
        }

        private static void LoadPosts()
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

            HttpRuntime.Cache.Insert("posts", posts);
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