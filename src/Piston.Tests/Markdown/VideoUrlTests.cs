namespace Piston.Tests.Markdown
{
    using Piston.Markdown;
    using Xunit;

    public class VideoUrlTests
    {
        private readonly VideoUrl _videoUrl;

        public VideoUrlTests()
        {
            _videoUrl = new VideoUrl();
        }

        [Fact]
        public void CanParseVideoUrl()
        {
            var result = _videoUrl.Transform(TestData.MarkdownWithVideo);

            Assert.Equal(@"# Heading

Lorem ipsum dolor sit amet, consectetur adipiscing elit.

<video src=""http://site.com/video.mp4"" />

Lorem ipsum dolor sit amet, consectetur adipiscing elit.", result);
        }

        [Fact]
        public void CanParseNoVideoUrl()
        {
            var result = _videoUrl.Transform(TestData.Markdown);

            Assert.Equal(result, TestData.Markdown);
        }

        [Fact]
        public void CanParseTextContainingVideoButNoUrl()
        {
            var text = @"# Heading

Lorem ipsum dolor sit amet video, consectetur adipiscing elit.

Lorem ipsum dolor sit amet, consectetur adipiscing elit.";

            var result = _videoUrl.Transform(text);

            Assert.Equal(result, text);
        }
    }
}
