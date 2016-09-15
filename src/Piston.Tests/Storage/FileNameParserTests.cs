namespace Piston.Tests.Storage
{
    using Piston.Storage;
    using System;
    using Xunit;

    public class FileNameParserTests
    {
        [Fact]
        public void ValidFileName()
        {
            var fileName = "2016-09-01-blog-post.md";
            FileNameMetadata metadata;

            Assert.True(FileNameParser.TryParseFileName(fileName, out metadata));
        }

        [Fact]
        public void InvalidFileName()
        {
            var fileName = "2016-blog-post.md";
            FileNameMetadata metadata;

            Assert.False(FileNameParser.TryParseFileName(fileName, out metadata));
        }

        [Fact]
        public void FileNameMetadataParsed()
        {
            var fileName = "2016-09-01-blog-post.md";
            FileNameMetadata metadata;

            FileNameParser.TryParseFileName(fileName, out metadata);

            var expected = new FileNameMetadata
            {
                Date = new DateTime(2016, 9, 1),
                Slug = "blog-post"
            };

            Assert.Equal(expected.Date, metadata.Date);
            Assert.Equal(expected.Slug, metadata.Slug);
        }
    }
}
