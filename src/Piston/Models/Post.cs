using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Piston.Models
{
    public enum Published
    {
        True = 0,
        Private = 1,
        Draft = 2,
    }

    public class Post
    {
        public Post()
        {
            Categories = Enumerable.Empty<string>();
        }

        public virtual void SetDefaultSettings()
        {
            Author = Piston.Settings.DefaultAuthor;
            Email = Piston.Settings.DefaultEmail;
        }

        public void SetHeaderSettings(Dictionary<string, object> settings)
        {
            foreach (var setting in settings)
            {
                switch (setting.Key.ToLower())
                {
                    case "categories":
                    case "category":
                        {
                            var categories = ((string)setting.Value).Split(
                                new[] { "," },
                                StringSplitOptions.RemoveEmptyEntries);

                            Categories = categories.Select(x => x.Trim()).OrderBy(x => x);

                            break;
                        }
                    case "title":
                        {
                            Title = (string)setting.Value;
                            break;
                        }
                    case "layout":
                        {
                            Layout = (string)setting.Value;
                            break;
                        }
                    case "author":
                        {
                            Author = (string)setting.Value;
                            break;
                        }
                    case "email":
                        {
                            Email = (string)setting.Value;
                            break;
                        }
                    case "published":
                        {
                            Published published;
                            Enum.TryParse((string)setting.Value, true, out published);
                            Published = published;
                            break;
                        }
                    case "metadescription":
                        {
                            MetaDescription = (string)setting.Value;
                            break;
                        }
                    case "tags":
                    case "keywords":
                        {
                            Keywords = (string)setting.Value;

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

        public Published Published { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Layout { get; set; }
        public IDictionary<string, dynamic> Settings { get; set; }
        public string FileName { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string Url { get; set; }

        public DateTime Date { get; set; }
    }
}