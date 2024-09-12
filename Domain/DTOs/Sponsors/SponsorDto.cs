using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Sponsors;
public class SponsorDto
{
    public Guid EventId { get; set; }
    //public int? SponsorMethodId { get; set; }
    public string? SponsorType { get; set; }
    public decimal? Amount { get; set; }
    [StringLength(200, ErrorMessage = "Message cannot exceed 200 characters.")]
    public string? Message { get; set; }
}
