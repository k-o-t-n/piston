using Piston.Storage;
using Xunit;

namespace Piston.Tests.Storage
{
    public class ContentLoaderTests
    {
        private readonly FakeFileReader _fakeFileReader;
        private readonly ContentLoader _contentLoader;

        private class FakeFileReader : IFileReader
        {
            private string _fileContent;

            public void SetFileContent(string fileContent)
            {
                _fileContent = fileContent;
            }

            public string ReadAllText(string filePath)
            {
                return _fileContent;
            }
        }

        public ContentLoaderTests()
        {
            _fakeFileReader = new FakeFileReader();
            _contentLoader = new ContentLoader(_fakeFileReader);
        }

        [Fact]
        public void CanParseFileWithNoHeader()
        {
            _fakeFileReader.SetFileContent(
                "# Some Heading\n" +
                "Some Content");

            var result = _contentLoader.ReadFile("/path/file.md");

            Assert.Equal("# Some Heading\nSome Content", result.Body);
            Assert.Equal("file.md", result.FileName);
            Assert.Equal(string.Empty, result.Header);
        }

        [Fact]
        public void CanParseFileWithHeader()
        {
            _fakeFileReader.SetFileContent(
                "---\n" +
                "key : value\n" +
                "key2 : value2\n" +
                "---\n" +
                "# Some Heading\n" +
                "Some Content");

            var result = _contentLoader.ReadFile("/path/file.md");

            Assert.Equal("\n# Some Heading\nSome Content", result.Body);
            Assert.Equal("file.md", result.FileName);
            Assert.Equal("---\nkey : value\nkey2 : value2\n---", result.Header);
        }

        [Fact]
        public void CanParseIncompleteHeader()
        {
            _fakeFileReader.SetFileContent(
                "---\n" +
                "key : value\n" +
                "key2 : value2\n" +
                "# Some Heading\n" +
                "Some Content");

            var result = _contentLoader.ReadFile("/path/file.md");

            Assert.Equal("---\nkey : value\nkey2 : value2\n# Some Heading\nSome Content", result.Body);
            Assert.Equal("file.md", result.FileName);
            Assert.Equal(string.Empty, result.Header);
        }
    }
}
