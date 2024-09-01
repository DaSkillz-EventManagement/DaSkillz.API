using System.Text.Json;

namespace Domain.Models.Response
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public string? Location { get; set; }
        public string? Detail { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
