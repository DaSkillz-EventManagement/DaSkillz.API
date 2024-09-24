namespace Domain.DTOs.Events.RequestDto
{
    public class FileUploadDto
    {
        public string base64 { get; set; } = string.Empty;
        public Guid eventId { get; set; }
        public string sponsorName { get; set; } = string.Empty;
    }
}
