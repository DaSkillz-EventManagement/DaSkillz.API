namespace Domain.DTOs.Payment.Response
{
    public class TransactionResponseDto
    {
        public string? Apptransid { get; set; }
        public string? Zptransid { get; set; }
        public string? Amount { get; set; }
        public string? Description { get; set; }
        public long Timestamp { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? EventId { get; set; }
        public Guid? UserId { get; set; }
    }
}
