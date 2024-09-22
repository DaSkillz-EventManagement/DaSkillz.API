namespace Domain.Entities;

public class Price
{
    public int PriceId { get; set; }
    public string PriceType { get; set; } = string.Empty;//ex: advertisement, premium, ticket,...
    public double amount { get; set; }
    public string note { get; set; } = string.Empty;
    public string status {  get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public virtual User? CreatedByNavigation { get; set; }

}
