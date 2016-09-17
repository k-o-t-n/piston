/*
 * Adapted from https://github.com/Sandra/Sandra.Snow
 * 
 * The MIT License
 * 
 * Copyright (c) 2013 Phillip Haydon, and contributors
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy 
 * of this software and associated documentation files (the "Software"), to deal 
 * in the Software without restriction, including without limitation the rights 
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
 * copies of the Software, and to permit persons to whom the Software is 
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all 
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
 * SOFTWARE.
 */

namespace Piston
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Piston.Models;

    public static class SlugExtensions
    {
        private static readonly SortedList<int, Func<string, Post, string>> UrlFormatParser = new SortedList
            <int, Func<string, Post, string>>
        {
            {0, DayFull},
            {1, DayAbbreviated},
            {2, Day},
            {3, MonthFull},
            {4, MonthAbbreviated},
            {5, Month},
            {6, YearFull},
            {7, Year},
            {8, Slug},
            {9, Category},
            {10, Author}
        };

        public static string ToUrlSlug(this string value)
        {
            //First to lower case
            value = value.ToLowerInvariant();

            //Remove all accents
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);

            value = Encoding.ASCII.GetString(bytes);

            //Replace spaces
            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

            //Replace ampersand
            value = value.Replace("&", "and");

            //Remove invalid chars
            value = Regex.Replace(value, @"[^a-z0-9\s-_\.]", "", RegexOptions.Compiled);

            //Trim dashes from end
            value = value.Trim('-', '_');

            //Replace double occurences of - or _
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }

        public static string AppendSlashIfNecessary(this string Url)
        {
            if (Url.IsFileUrl()) // URL is to a file
            {
                return Url;
            }
            else // URL is to a directory
            {
                return Url + "/";
            }
        }

        public static bool IsFileUrl(this string Url)
        {
            return Url.Contains('/') && Url.Split('/').Last().Contains('.');
        }

        public static void SetPostUrl(this IEnumerable<Post> posts)
        {
            foreach (var postHeader in posts)
            {
                var urlFormat = Settings.PostUrlFormat.Trim('/').AppendSlashIfNecessary();

                foreach (var s in UrlFormatParser.OrderBy(x => x.Key).Select(x => x.Value))
                {
                    urlFormat = s.Invoke(urlFormat, postHeader);
                }

                postHeader.Url = urlFormat;
            }
        }

        private static string DayFull(string url, Post post)
        {
            return url.Replace("dddd", post.Date.ToString("dddd"));
        }

        private static string DayAbbreviated(string url, Post post)
        {
            return url.Replace("ddd", post.Date.ToString("ddd"));
        }

        private static string Day(string url, Post post)
        {
            return url.Replace("dd", post.Date.ToString("dd"));
        }

        private static string Month(string url, Post post)
        {
            return url.Replace("MM", post.Date.ToString("MM"));
        }

        private static string MonthAbbreviated(string url, Post post)
        {
            return url.Replace("MMM", post.Date.ToString("MMM"));
        }

        private static string MonthFull(string url, Post post)
        {
            return url.Replace("MMMM", post.Date.ToString("MMMM"));
        }

        private static string YearFull(string url, Post post)
        {
            return url.Replace("yyyy", post.Date.ToString("yyyy"));
        }

        private static string Year(string url, Post post)
        {
            return url.Replace("yy", post.Date.ToString("yy"));
        }

        private static string Slug(string url, Post post)
        {
            return url.Replace("{slug}", post.Url).Replace("slug", post.Url);
        }

        private static string Category(string url, Post post)
        {
            return url.Replace("{category}", post.Categories.FirstOrDefault());
        }

        private static string Author(string url, Post post)
        {
            return url.Replace("{author}", post.Author);
        }
    }
}