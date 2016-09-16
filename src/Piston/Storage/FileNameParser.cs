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