namespace Piston.Tests.Markdown
{
    internal class TestData
    {
        public const string EmptyString = "";

        public const string Null = null;

        public const string Markdown = @"# Heading

Lorem ipsum dolor sit amet, consectetur adipiscing elit.";

        public const string MetadataMarkdown = @"key1: value
key2: value
key3: value

# Heading

Lorem ipsum dolor sit amet, consectetur adipiscing elit.";

        public const string NotMarkdown = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.";
    }
}