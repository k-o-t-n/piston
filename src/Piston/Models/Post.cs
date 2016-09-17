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

namespace Piston.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Post
    {
        public Post()
        {
            Categories = Enumerable.Empty<string>();
            Author = Settings.DefaultAuthor;
            Email = Settings.DefaultEmail;
            Layout = "post";
        }

        public void LoadMetadata(IEnumerable<KeyValuePair<string, string>> metadata)
        {
            foreach (var setting in metadata)
            {
                switch (setting.Key.ToLower())
                {
                    case "categories":
                    case "category":
                        {
                            var categories = setting.Value.Split(
                                new[] { "," },
                                StringSplitOptions.RemoveEmptyEntries);

                            Categories = categories.Select(x => x.Trim()).OrderBy(x => x);

                            break;
                        }
                    case "title":
                        {
                            Title = setting.Value;
                            break;
                        }
                    case "layout":
                        {
                            Layout = setting.Value.ToLowerInvariant();
                            break;
                        }
                    case "author":
                        {
                            Author = setting.Value;
                            break;
                        }
                    case "email":
                        {
                            Email = setting.Value;
                            break;
                        }
                    case "published":
                        {
                            IsPublished = new [] { "true", "published" }.Contains(setting.Value.ToLowerInvariant());
                            break;
                        }
                    case "metadescription":
                        {
                            MetaDescription = setting.Value;
                            break;
                        }
                    case "tags":
                    case "keywords":
                        {
                            Keywords = setting.Value;

                            break;
                        }
                    case "score":
                        {
                            int score;
                            if (int.TryParse(setting.Value, out score))
                            {
                                Score = score;
                            }
                            else
                            {
                                Score = 0;
                            }

                            break;
                        }
                }
            }
        }

        public string MetaDescription { get; set; }

        public string Author { get; set; }
        public string Email { get; set; }

        public IEnumerable<string> Categories { get; set; }
        public string Keywords { get; set; }

        public bool IsPublished { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Layout { get; set; }

        public string Preview
        {
            get
            {
                return Content.Substring(0, Content.IndexOf("</p>", StringComparison.OrdinalIgnoreCase) + 4);
            }
        }

        public string FileName { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string Url { get; set; }

        public DateTime Date { get; set; }

        public int Score { get; set; }
    }
}