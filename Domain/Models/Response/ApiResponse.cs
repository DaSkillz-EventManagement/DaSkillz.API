using System.Net;

namespace Domain.Models.Response
{
    public class APIResponse
    {
        public HttpStatusCode StatusResponse { get; set; }
        public string? Message { get; set; } = string.Empty;
        public object? Data { get; set; }
    }
}
