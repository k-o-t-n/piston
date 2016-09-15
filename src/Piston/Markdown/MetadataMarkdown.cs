/*
 * Adapted from https://github.com/bradwestness/MetadataMarkdownSharp
 * 
 * The MIT License (MIT)
 * 
 * Copyright (c) 2014 Brad Westness
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

namespace Piston.Markdown
{
    using System.Collections.Generic;
    using HeyRed.MarkdownSharp;
    using System.Text;
    using System.IO;

    /// <summary>
    /// A wrapper for MarkdownSharp to add MultiMarkdown style metadata
    /// </summary>
    public class MetadataMarkdown : Markdown
    {
        public MetadataMarkdown()
        {

        }

        public MetadataMarkdown(MarkdownOptions options) : base(options)
        {

        }

        /// <summary>
        /// Overrides the MarkdownSharp.Markdown class's .Transform() method,
        /// but omits the MetadataMarkdown metadata from the text before
        /// sending it to MarkdownSharp for transformation.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public new string Transform(string text)
        {
            var metadataSection = GetMetadataSection(text);

            if (string.IsNullOrWhiteSpace(metadataSection))
            {
                return base.Transform(text);
            }

            var markdownSection = text.Substring(metadataSection.Length);

            return base.Transform(markdownSection);
        }

        /// <summary>
        /// Parses the metadata section at the head of a markdown document
        /// into a collection if key/value pairs
        /// </summary>
        /// <param name="text">A string containing some Markdown with or without metadata</param>
        /// <returns>A list of key/value pairs of all metadata items at the top of the markdown text</returns>
        public IEnumerable<KeyValuePair<string, string>> Metadata(string text)
        {
            var metadataSection = GetMetadataSection(text);
            var metadata = ParseMetadataSection(metadataSection);
            return metadata;
        }

        private static string GetMetadataSection(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            var output = new StringBuilder();

            using (var sr = new StringReader(input))
            {
                while (sr.Peek() != -1)
                {
                    var line = sr.ReadLine();

                    if (!line.Contains(":"))
                    {
                        // metadata lines must contain a colon to separate the key from the value
                        break;
                    }

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        // metadata section ends at the first blank line
                        break;
                    }

                    output.AppendLine(line);
                }
            }

            return output.ToString();
        }

        private static IEnumerable<KeyValuePair<string, string>> ParseMetadataSection(string input)
        {
            IList<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();

            if (!string.IsNullOrWhiteSpace(input))
            {
                using (var sr = new StringReader(input))
                {
                    while (sr.Peek() != -1)
                    {
                        var line = sr.ReadLine();

                        // we already verified that the line contains a colon
                        // so this should never be negative or out of range
                        var keyValueSeparatorPosition = line.IndexOf(':');
                        var key = line.Substring(0, keyValueSeparatorPosition).Trim();
                        var value = line.Substring(keyValueSeparatorPosition + 1).Trim();
                        keyValuePairs.Add(new KeyValuePair<string, string>(key, value));
                    }
                }
            }

            return keyValuePairs;
        }

    }
}