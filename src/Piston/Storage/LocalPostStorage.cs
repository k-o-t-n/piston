namespace Piston.Storage
{
    using System.Collections.Generic;
    using System.Linq;
    using Models;
    using System.Web.Hosting;
    using Markdown;
    using System.IO;

    internal class LocalPostStorage : IPostStorage
    {
        private static readonly string _posts = HostingEnvironment.MapPath("~/posts/");
        private readonly MetadataMarkdown _mark;

        public LocalPostStorage(MetadataMarkdown mark)
        {
            _mark = mark;
        }

        public IEnumerable<Post> GetAllPosts()
        {
            var posts = new List<Post>();

            foreach (var filePath in Directory.EnumerateFiles(_posts))
            {
                var fileName = Path.GetFileName(filePath);
                FileNameMetadata metadata;                

                if (!FileNameParser.TryParseFileName(fileName, out metadata))
                {
                    System.Diagnostics.Debug.WriteLine($"Skipping file {fileName}");
                    break;
                }

                var fileContent = File.ReadAllText(filePath);

                var post = new Post
                {
                    FileName = fileName,
                    Content = _mark.Transform(fileContent),
                    Year = metadata.Date.Year,
                    Month = metadata.Date.Month,
                    Day = metadata.Date.Day,
                    Date = metadata.Date,
                    Url = metadata.Slug
                };

                post.LoadMetadata(_mark.Metadata(fileContent));

                posts.Add(post);
            }

            posts.SetPostUrl();

            return posts.AsEnumerable();
        }
    }
}