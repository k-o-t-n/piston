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
                            Layout = setting.Value;
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

        public string FileName { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string Url { get; set; }

        public DateTime Date { get; set; }
    }
}