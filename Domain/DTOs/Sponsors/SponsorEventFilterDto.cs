

using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Sponsors;
public class SponsorEventFilterDto
{
    [Required]
    public Guid EventId { get; set; }
    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;
    [Range(1, int.MaxValue)]
    public int EachPage { get; set; } = 10;
    public string? Status { get; set; }
    public string? SponsorType { get; set; }
    public bool? IsSponsored { get; set; }
}
