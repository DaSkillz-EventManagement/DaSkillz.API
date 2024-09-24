namespace Domain.DTOs.Payment.Response
{
    public class TransactionResponseDto
    {
        public string? AppTransId { get; set; }
        public string? ZpTransId { get; set; }
        public string? Amount { get; set; }
        public string? Description { get; set; }
        public long Timestamp { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? EventId { get; set; }
        public Guid? UserId { get; set; }
        public string? UserEmail { get; set; }
        public string? EventName { get; set; }
        public int SubscriptionType { get; set; }
        public string OrderUrl { get; set; } 
    }
}
