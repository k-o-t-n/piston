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

namespace Piston.Storage
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;

    public static class FileNameParser
    {
        private static readonly Regex FileNameRegex =
            new Regex(@"^(?<year>\d{4})-(?<month>\d{2})-(?<day>\d{2})-(?<slug>.+).(?:md|markdown)$",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static bool TryParseFileName(string fileName, out FileNameMetadata metadata)
        {
            var fileNameMatches = FileNameRegex.Match(fileName);

            if (fileNameMatches.Success)
            {
                var year = fileNameMatches.Groups["year"].Value;
                var month = fileNameMatches.Groups["month"].Value;
                var day = fileNameMatches.Groups["day"].Value;
                var slug = fileNameMatches.Groups["slug"].Value.ToUrlSlug();
                var date = DateTime.ParseExact(year + month + day, "yyyyMMdd", CultureInfo.InvariantCulture);

                metadata = new FileNameMetadata
                {
                    Slug = slug,
                    Date = date
                };

                return true;
            }

            metadata = new FileNameMetadata();
            return false;
        }
    }
}