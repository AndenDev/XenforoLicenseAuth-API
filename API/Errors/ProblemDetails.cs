namespace API.Errors
{
    public class ProblemDetails
    {
        public string Type { get; set; } = "about:blank";
        public string Title { get; set; }
        public int? Status { get; set; }
        public string Detail { get; set; }
        public string Instance { get; set; }
        public IDictionary<string, object> Extensions { get; } = new Dictionary<string, object>();
    }
}
