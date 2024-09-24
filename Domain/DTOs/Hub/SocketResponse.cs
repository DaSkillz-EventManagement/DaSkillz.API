namespace Domain.DTOs.Hub;

public class SocketResponse
{
    public bool StatusResponse { get; set; }
    public string? Message { get; set; } = string.Empty;
    public object? Data { get; set; }
}
