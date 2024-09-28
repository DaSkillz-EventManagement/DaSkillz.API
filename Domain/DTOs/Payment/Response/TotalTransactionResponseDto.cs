namespace Domain.DTOs.Payment.Response
{
    public class TotalTransactionResponseDto
    {
        public Guid? EventId { get; set; }
        public Guid? UserId { get; set; }
        public decimal? TotalCommissionCharge { get; set; }
        public decimal? TotalTicketPaidAmount { get; set; }
        public decimal? TotalSponsorAmount { get; set; }
       
    }
}
