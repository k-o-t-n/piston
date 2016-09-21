namespace Piston.Tests.Markdown
{
    using Piston.Markdown;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class VideoUrlTests
    {
        [Fact]
        public void CanParseVideoUrl()
        {
            var videoUrl = new VideoUrl();

            var result = videoUrl.Transform(TestData.MarkdownWithVideo);

            Assert.Equal(@"# Heading

Lorem ipsum dolor sit amet, consectetur adipiscing elit.

<video src=""http://site.com/video.mp4"" />

Lorem ipsum dolor sit amet, consectetur adipiscing elit.", result);
        }

        [Fact]
        public void CanParseNoVideoUrl()
        {
            var videoUrl = new VideoUrl();

            var result = videoUrl.Transform(TestData.Markdown);

            Assert.Equal(result, TestData.Markdown);
        }
    }
}
