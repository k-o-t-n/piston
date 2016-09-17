namespace Piston.Models
{
    using System.Collections.Generic;

    public class Page
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string Url { get; set; }

        public void LoadMetadata(IEnumerable<KeyValuePair<string, string>> metadata)
        {
            foreach (var setting in metadata)
            {
                switch (setting.Key.ToLower())
                {
                    case "title":
                        {
                            Title = setting.Value;
                            break;
                        }
                }
            }
        }
    }
}