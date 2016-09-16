namespace Piston
{
    using System.Configuration;

    public static class Settings
    {
        static Settings()
        {
            Title = ConfigurationManager.AppSettings.Get("piston:name");
            Description = ConfigurationManager.AppSettings.Get("piston:description");
            PostsPerPage = int.Parse(ConfigurationManager.AppSettings.Get("piston:postsPerPage"));
            DefaultAuthor = ConfigurationManager.AppSettings.Get("piston:defaultAuthor");
            DefaultEmail = ConfigurationManager.AppSettings.Get("piston:defaultEmail");
            PostUrlFormat = ConfigurationManager.AppSettings.Get("piston:postUrlFormat");
            MaxScore = int.Parse(ConfigurationManager.AppSettings.Get("piston:maxScore"));
        }

        public static string Title { get; private set; }
        public static string Description { get; private set; }
        public static int PostsPerPage { get; private set; }
        public static string DefaultAuthor { get; private set; }
        public static string DefaultEmail { get; private set; }
        public static string PostUrlFormat { get; private set; }
        public static int MaxScore { get; private set; }
    }
}