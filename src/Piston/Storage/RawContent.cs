namespace Piston.Storage
{
    public class RawContent
    {
        public RawContent()
        {
            FileName = string.Empty;
            Header = string.Empty;
            Body = string.Empty;
        }

        public string FileName { get; set; }

        public string Header { get; set; }

        public string Body { get; set; }
    }
}