namespace Domain.DTOs.Certificate
{
    public class CertificateResponseDto
    {
        public int certicateId {  get; set; }
        public Guid UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public Guid EventId { get; set; }
        public string? EventName { get; set; }
        public DateTime IssueDate { get; set; }
    }
}
