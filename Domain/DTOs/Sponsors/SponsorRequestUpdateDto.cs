namespace Domain.DTOs.Sponsors;

public class SponsorRequestUpdateDto
{
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public string? Status { get; set; }
}
