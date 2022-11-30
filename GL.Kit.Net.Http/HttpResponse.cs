namespace System.Net.Http
{
    public class HttpResponse
    {
        public bool IsSuccessStatusCode { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public string Authorization { get; set; }

        public string Content { get; set; }

        public string ReasonPhrase { get; set; }
    }
}
