namespace MTCGServer.Core.Request
{
    public class Request
    {
        public HttpMethod Method { get; set; }
        public string ResourcePath { get; set; }
        public string HttpVersion { get; set; }
        public Dictionary<string, string> Header { get; set; }
        public string? Payload { get; set; }

        public Request()
        {
            Method = HttpMethod.Get;
            ResourcePath = "";
            HttpVersion = "HTTP/1.1";
            Header = new Dictionary<string, string>();
            Payload = null;
        }
    }
}
