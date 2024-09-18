namespace Domain.Entities
{
    public class RefundTransaction
    {
        public long Id { get; set; }
        public long refundId { get; set; }
        public int returnCode { get; set; }
        public string? returnMessage { get; set; }
        public long refundAmount { get; set; }
        public DateTime refundAt { get; set; }
        public string? Apptransid { get; set; }

        public virtual Transaction? Transaction { get; set; }
    }
}
