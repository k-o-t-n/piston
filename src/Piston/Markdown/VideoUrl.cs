namespace Piston.Markdown
{
    using HeyRed.MarkdownSharp;
    using System.Text.RegularExpressions;

    public class VideoUrl : IMarkdownExtension
    {
        private static Regex VideoRegex = new Regex(@"@\[video\]\((?<url>.+)\)", 
            RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        public string Transform(string text)
        {
            if (!text.Contains("@[video]"))
            {
                return text;
            }

            return VideoRegex.Replace(text, VideoRegexMatchEvaluator);
        }

        private static string VideoRegexMatchEvaluator(Match match)
        {
            var url = match.Groups["url"].Value;
            return $"<video src=\"{url}\" />";
        }
    }
}